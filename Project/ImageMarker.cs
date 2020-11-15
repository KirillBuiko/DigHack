using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;


public class GImageMarker : GMapMarker
{
    int ID;
    public GMapOverlay cuda;
    public Bitmap Bitmap { get; set; }
    public GImageMarker(PointLatLng p, string path, string podpis, GMapControl map, int id, bool brt)
        : base(p)
    {
        Bitmap pic = new Bitmap(path);
        this.Bitmap = pic;
        ID = id;
        Size = new System.Drawing.Size(pic.Width, pic.Height);
        Offset = new Point(-Size.Width / 2, -Size.Height);
        cuda = new GMapOverlay(map, Convert.ToString(id));
        if (brt)
            this.ToolTipText = podpis;
        else
        {
            this.ToolTipText = podpis + "\n" + "(" + Convert.ToString(p.Lat) + "; " + Convert.ToString(p.Lng) + ") ";
        }
        this.ToolTip =
        new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(this);
        cuda.Markers.Add(this);
        map.Overlays.Add(cuda);
    }
    public string GetText()
    {
        return this.ToolTipText;
    }

   
    public override void OnRender(Graphics g)
    {
        g.DrawImage(Bitmap, LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height);
    }
}
