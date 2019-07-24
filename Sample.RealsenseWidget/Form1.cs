using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RealsenseWrapper.Form;
namespace Sample.RealsenseWidget
{
    public partial class Form1 : Form
    {
        RealsenseWrapper.Form.RealsenseWidget
             widget;
        public Form1()
        {
            InitializeComponent();
            widget = new RealsenseWrapper.Form.RealsenseWidget
               // (pictureBox1, pictureBox2, pictureBox3, openGLControl1);
               (pictureBox1, pictureBox2, null, null);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            widget.frameStep = 0;
            widget.StartDisplay();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            widget.StopDisplay();
        }
    }
}
