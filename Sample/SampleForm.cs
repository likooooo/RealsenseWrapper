using RealsenseWrapper;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sample
{
    public partial class SampleForm : Form
    {
        RealsenseControl rs;
        public SampleForm()
        {
            InitializeComponent();

            try
            {
                rs = new RealsenseControl();
                rs.ColorFrameEnable = true;
                rs.DepthFrameEnalbe = true;
                rs.InfraredLeftEnalbe = true;
                rs.InfraredRightEnalbe = true;
                rs.StartWithCfg();
                tmDisplay.Start();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.HResult.ToString()+":未检测到可用相机，异常退出");
            }
        }

        private void tmDisplay_Tick(object sender, EventArgs e)
        {
            if (rs.UpdateFrame())
            {
                rs.Aligin();
                var rgb = rs.GetImage(ModuleStream.Color);
                picRgb.Image = rgb.GetBitmap();
                picRgb.Refresh();
                var depth = rs.GetImage(ModuleStream.Depth);
                picDepth.Image = depth.GetBitmap();
                picDepth.Refresh();
                var infrared1 = rs.GetImage(ModuleStream.Infrared, 1);
                picInfrared.Image = infrared1.GetBitmap();
                picInfrared.Refresh();
                var texture = (Bitmap)picRgb.Image;
                rs.GetPointclouds();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmDisplay.Stop();
            rs.Stop();       
        }
    }
}
