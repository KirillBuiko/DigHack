using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;


namespace window3
{
    public partial class mapForm : Form
    {
        public mapForm()
        {
            InitializeComponent();
        }
        WebRequests web = new WebRequests("");
        string[] NamesOfPNG =
        {
            "GorVodichka.png",
            "HolVodichka2.png",
            "DTP2.png",
            "Ogonok.png",
        };
        List<GImageMarker> Mar = new List<GImageMarker>();
        GMap.NET.WindowsForms.GMapOverlay markersOverlay;
        public void DrawPoly(List<Point>[] points, string describtion, Color penColor, Color InColor, GMapControl map, int count, List<GMapPolygon> poly)
        {
            for (int i = 0; i < count; i++)
            {
                if(points[i].Count==0)
                {
                    break;
                }
               
                List<PointLatLng> latLngs = new List<PointLatLng>();
                foreach (Point p in points[i])
                {
                    latLngs.Add(new PointLatLng(p.latitude, p.longitude));
                }
                
                poly.Add(new GMapPolygon(latLngs, describtion));
                poly.Last().Stroke = new Pen(penColor, 3);
                poly.Last().Fill = new SolidBrush(InColor);
                
                GMapOverlay overlay = new GMapOverlay(map, "poly");
               
                if (i == 0)
                {
                    string disribtion = "В данном полигоне " + Convert.ToString(points[i].Count) + " очагов с \n соответвующими координатами: \n";
                    for(int j=0; j< points[i].Count; j++)
                    {
                        disribtion+= Convert.ToString(j+1)+"( "+ Convert.ToString(points[i].ElementAt(j).latitude)+"; " + Convert.ToString(points[i].ElementAt(j).longitude) + ")\n";
                    }
                    switch (points[0].ElementAt(i).type)
                    {
                        case 1:
                            {
                                Mar.Add(new GImageMarker(new PointLatLng(points[0].ElementAt(i).latitude, points[0].ElementAt(i).longitude),
                                                            NamesOfPNG[0], disribtion, map, points[0].ElementAt(i).id, true));
                                break;
                            }
                        case 2:
                            {
                                Mar.Add(new GImageMarker(new PointLatLng(points[0].ElementAt(i).latitude, points.ElementAt(i)[0].longitude),
                                                            NamesOfPNG[1], disribtion, map, points[0].ElementAt(i).id, true));
                                break;
                            }
                        case 3:
                            {
                                Mar.Add(new GImageMarker(new PointLatLng(points[0].ElementAt(i).latitude, points[0].ElementAt(i).longitude),
                                                            NamesOfPNG[2], disribtion, map, points[0].ElementAt(i).id, true));
                                break;
                            }
                        case 4:
                            {
                                Mar.Add(new GImageMarker(new PointLatLng(points[0].ElementAt(i).latitude, points[0].ElementAt(i).longitude),
                                                            NamesOfPNG[3], disribtion, map, points[0].ElementAt(i).id, true));
                                break;
                            }
                    }
                }
                Mar.ElementAt(0).cuda.Polygons.Add(poly.Last());
                //overlay.Polygons.Add(poly.Last());
                map.Overlays.Add(Mar.ElementAt(0).cuda);
            }
        }
        public double metr = 0.000017865;
       public int Holpoly = 0;
       public int Firepoly = 0;
       public int Gorpoly = 0;
       public int DTPpoly = 0;

        List<GMapPolygon> GPoly = new List<GMapPolygon>();
        List<GMapPolygon> HPoly = new List<GMapPolygon>();
        List<GMapPolygon> DPoly = new List<GMapPolygon>();
        List<GMapPolygon> FPoly = new List<GMapPolygon>();

        List<Point> points;

        List<Point>[] PolysListHolVod =  new List<Point>[100];
        List<Point>[] PolysListGorVod = new List<Point>[100];
        List<Point>[] PolysListDTP = new List<Point>[100];
        List<Point>[] PolysListFire = new List<Point>[100];
        private void mapForm_Load(object sender, EventArgs e)
        {
            //Настройки для компонента GMap.
            map.Bearing = 0;

            //CanDragMap - Если параметр установлен в True,
            //пользователь может перетаскивать карту
            ///с помощью правой кнопки мыши.
            map.CanDragMap = true;

            //Указываем, что перетаскивание карты осуществляется
            //с использованием левой клавишей мыши.
            //По умолчанию - правая.
            map.DragButton = MouseButtons.Left;

            map.GrayScaleMode = true;

            //MarkersEnabled - Если параметр установлен в True,
            //любые маркеры, заданные вручную будет показаны.
            //Если нет, они не появятся.
            map.MarkersEnabled = true;

            //Указываем значение максимального приближения.
            map.MaxZoom = 18;

            //Указываем значение минимального приближения.
            map.MinZoom = 2;

            //Устанавливаем центр приближения/удаления
            //курсор мыши.
            map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;

            //Отказываемся от негативного режима.
            map.NegativeMode = false;

            //Разрешаем полигоны.
            map.PolygonsEnabled = true;

            //Разрешаем маршруты
            map.RoutesEnabled = true;

            //Скрываем внешнюю сетку карты
            //с заголовками.
            map.ShowTileGridLines = false;

            //Указываем, что при загрузке карты будет использоваться
            //18ти кратное приближение.
            map.Zoom = 5;

            //Указываем что все края элемента управления
            //закрепляются у краев содержащего его элемента
            //управления(главной формы), а их размеры изменяются
            //соответствующим образом.
            map.Dock = DockStyle.Fill;

            //Указываем что будем использовать карты Google.
            map.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;

            map.Position = new GMap.NET.PointLatLng(59.945053, 30.341369);

            //Создаем новый список маркеров, с указанием компонента
            //в котором они будут использоваться и названием списка
            markersOverlay =
            new GMap.NET.WindowsForms.GMapOverlay(map, "marker");
            map.OnMarkerClick += gMapControl1_OnMarkerClick;

            InitLists(points, PolysListGorVod, 200, 1, ref Gorpoly);
            InitLists(points, PolysListHolVod, 100, 2, ref Holpoly);
            InitLists(points, PolysListDTP, 500, 3, ref DTPpoly);
            InitLists(points, PolysListFire, 500, 4, ref Firepoly);

            DrawPoly(PolysListGorVod, "Полигон горячей воды.", Color.Blue, Color.Red, map, Gorpoly, GPoly);
            DrawPoly(PolysListHolVod, "Полигон холодной воды.", Color.Red, Color.Blue, map, Holpoly, HPoly);
            DrawPoly(PolysListDTP, "Полигон ДТП.", Color.Black, Color.Gray, map, DTPpoly, DPoly);
            DrawPoly(PolysListFire, "Полигон пожаров.", Color.Blue, Color.Red, map, Firepoly, FPoly);

            InitMarkers(Mar, points);

            //Если вы используете интернет через прокси сервер,
            //указываем свои учетные данные.
            GMap.NET.MapProviders.GMapProvider.WebProxy = System.Net.WebRequest.GetSystemWebProxy();
            GMap.NET.MapProviders.GMapProvider.WebProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Controls.Add(map);
        }
        void InitLists(List<Point> from, List<Point>[] to, int m, int type, ref int count)
        {
            for (int i = 0; i < from.Count; i++)
            {
                if (from[i].type != type)
                {
                    continue;
                }
                foreach (List<Point> NewP in to)
                {
                    if (NewP.Count != 0)
                    {
                        for (int j = 0; j < NewP.Count; j++)
                        {
                            if (i >= from.Count)
                                i = from.Count - 1;
                            if (from.Count == 0)
                                break;
                            if (NewP[j].type != from[i].type)
                                break;
                            if (Math.Abs(from[i].latitude - NewP[j].latitude) <= m * metr && Math.Abs(from[i].latitude - NewP[j].latitude) <= m * metr)
                            {
                                NewP.Add(from[i]);
                                from.Remove(from[i]);
                            }
                        }
                    }
                    else
                    {
                        NewP.Add(from[i]);
                        from.Remove(from[i]);
                    }
                }
            }
            foreach (List<Point> NewP in to)
            {
                if (NewP != null)
                {
                    if (NewP.Count == 1)
                    {
                        for (int i = 0; i < NewP.Count; i++)
                        {
                            from.Add(NewP[i]);
                            NewP.Remove(NewP[i]);
                        }
                    }
                    count++;
                }
            }
        }       
  
        public void InitMarkers(List<GImageMarker> Mar, List<Point> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                switch (points.ElementAt(i).type)
                {
                    case 1:
                        {
                            Mar.Add(new GImageMarker(new PointLatLng(points.ElementAt(i).latitude, points.ElementAt(i).longitude),
                                                        NamesOfPNG[0], "Горячая вода", map, points.ElementAt(i).id, false));
                            break;
                        }
                    case 2:
                        {
                            Mar.Add(new GImageMarker(new PointLatLng(points.ElementAt(i).latitude, points.ElementAt(i).longitude),
                                                        NamesOfPNG[1], "Холодная вода", map, points.ElementAt(i).id, false));
                            break;
                        }
                    case 3:
                        {
                            Mar.Add(new GImageMarker(new PointLatLng(points.ElementAt(i).latitude, points.ElementAt(i).longitude),
                                                        NamesOfPNG[2], "ДТП", map, points.ElementAt(i).id, false));
                            break;
                        }
                    case 4:
                        {
                            Mar.Add(new GImageMarker(new PointLatLng(points.ElementAt(i).latitude, points.ElementAt(i).longitude),
                                                        NamesOfPNG[3], "Пожар", map, points.ElementAt(i).id, false));
                            break;
                        }
                }

            }
        }
        private void Map_Load(object sender, EventArgs e)
        {
            WebRequests web = new WebRequests("http://25.62.84.86/spb");
            points = web.getNotDonePoints();
        }
        void gMapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            string message =
"Вы уверены, что хотите удалить маркер происшествия: " + item.ToolTipText + " ?";
            const string caption = "Решена ли задача?";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {
                for (int i = 0; i < Mar.Count; i++)
                {
                    _ = Mar.ElementAt(i).cuda.Markers.Remove(item);
                }

                for (int i = 0; i < GPoly.Count; i++)
                {
                    if (GPoly.ElementAt(i).Points.ElementAt(0).Lat == item.Position.Lat && GPoly.ElementAt(i).Points.ElementAt(0).Lng == item.Position.Lng)
                    {
                        Mar.ElementAt(i).cuda.Polygons.Remove(GPoly.ElementAt(i));
                    }
                }

                for (int i = 0; i < HPoly.Count; i++)
                {
                    if (HPoly.ElementAt(i).Points.ElementAt(0).Lat == item.Position.Lat && HPoly.ElementAt(i).Points.ElementAt(0).Lng == item.Position.Lng)
                    {
                        Mar.ElementAt(i).cuda.Polygons.Remove(HPoly.ElementAt(i));
                    }
                }

                for (int i = 0; i < DPoly.Count; i++)
                {
                    if (DPoly.ElementAt(i).Points.ElementAt(0).Lat == item.Position.Lat && DPoly.ElementAt(i).Points.ElementAt(0).Lng == item.Position.Lng)
                    {
                        Mar.ElementAt(i).cuda.Polygons.Remove(DPoly.ElementAt(i));
                    }
                }

                for (int i = 0; i < FPoly.Count; i++)
                {
                    if (FPoly.ElementAt(i).Points.ElementAt(0).Lat == item.Position.Lat && FPoly.ElementAt(i).Points.ElementAt(0).Lng == item.Position.Lng)
                    {
                        Mar.ElementAt(i).cuda.Polygons.Remove(FPoly.ElementAt(i));
                    }
                }
            }
        }
        private void Map_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void Map_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            Console.WriteLine(item.ToolTipText);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
