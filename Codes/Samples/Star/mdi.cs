using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Star
{
    public partial class mdi : Form
    {
        public mdi()
        {
            InitializeComponent();
            ShowStar();
        }

        public void ShowStar()
        {
            Form1 f = new Form1();
            f.StarMode();
            f.MdiParent = this;

            f.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ShowStar();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Width = 1000;
            f.Height = 300;
            f.RowMode();
            f.MdiParent = this;

            f.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            
            f.GridMode();
            f.MdiParent = this;

            f.Show();
        }
    }
}
