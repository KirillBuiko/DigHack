using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace window3
{

    public partial class mainForm : Form
    {
        bool flag = false;
        mapForm map = new mapForm();

        public mainForm()
        {
            InitializeComponent();
        }
        
        //Закрытие формы входа
        public void loginForm_FormClosed(object sender, EventArgs e)
        {
            this.Close();
        }


        private void mainForm_Load(object sender, EventArgs e)
        {
        }

        //Разворачивание из трея при двойной нажатии на иконку
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        //Сворачивание приложения в трей при нажатии на крестик
        public void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            e.Cancel = true;
            if (flag == true)
            {
                e.Cancel = false;
                flag = false;
            }
        }
        
        //При нажатии на кнопку открытие формы регистрации 

        //При входе в учетную запись на главном экране переопределяются кнопки
        //"Вход" => "Телеметрия"
        //"Регистрация" => "Личный кабинет"

        //При нажатии открывается форма Телеметрия

        //Завершение работы из главного экрана 
        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = true;
            this.Close();
        }
        
        //При нажатии открывается личный кабинет при условии входа в учетную запись

        //Завершение работы из трея
        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = true;
            this.Close();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            mapForm map1 = new mapForm();
            map1.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            WebRequests web = new WebRequests("http://25.62.84.86/spb");
            web.addOne();
        }
    }
}
