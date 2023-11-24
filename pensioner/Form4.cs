using pensioner2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pensioner
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            SetFullScreen();
        }
        private void SetFullScreen()
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.WindowState = FormWindowState.Maximized;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (GlobalData.FontText == 20)
                GlobalData.FontText = 16;
            else
                GlobalData.FontText = 20;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
