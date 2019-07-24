using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpglWrapper;
using System.Drawing;
using SharpGL;
using System.Threading;
namespace RealsenseWrapper.Form
{
    public delegate void DisplayImg(Bitmap bitmap);
    public class RealsenseWidget
    {
        RealsenseControl rs;
        PictureBox picRgb, picDepth, picInfrared;
        OpenGLControl openGLControl;
        SharpglWrapper.SharpglWrapper sharpglControl;
        Thread displayThread;
        public int frameStep;
        private void InitCamera()
        {
            try
            {
                rs = new RealsenseControl();
                rs.ColorFrameEnable = true;
                rs.DepthFrameEnalbe = true;
                rs.InfraredLeftEnalbe = true;
                rs.InfraredRightEnalbe = true;
                rs.StartWithCfg();
            }
            catch (Exception e)
            {
                MessageBox.Show("未检测到可用相机，异常退出");
            }
        }
        private void InitOpengl()
        {
            sharpglControl = new SharpglWrapper.SharpglWrapper(openGLControl);        
        }
        void DisplayRgb(Bitmap bitmap)
        {
            picRgb.Image = bitmap;
        }
        void DisplayInfrared(Bitmap bitmap)
        {
            picInfrared.Image = bitmap;
        }
        void DisplayDepth(Bitmap bitmap)
        {
            picDepth.Image = bitmap;
        }
        void DisplayPointcloud(Bitmap bitmap)
        {
         
        }
        private void LoopCapture()
        {
            DisplayImg displayRgb=  new DisplayImg(DisplayRgb);
            DisplayImg displayInfrared = new DisplayImg(DisplayInfrared);
            DisplayImg displayDepth = new DisplayImg(DisplayDepth);
            while (true)
            {
                if (rs.UpdateFrame())
                {
                    rs.Aligin();
                    if (null != picRgb)
                    {
                        var rgb = rs.GetImage(ModuleStream.Color);
                        picRgb.BeginInvoke(displayRgb
                            , new object[1] { rgb.GetBitmap() });
                    }
                    if (null != picDepth)
                    {
                        var depth = rs.GetImage(ModuleStream.Depth);
                        picDepth.BeginInvoke(displayDepth
                              , new object[1] { depth.GetBitmap() });
                    }
                    if (null != picInfrared)
                    {
                        var infrared1 = rs.GetImage(ModuleStream.Infrared, 1);
                        picInfrared.BeginInvoke(displayInfrared
                                            , new object[1] { infrared1.GetBitmap() });
                    }
                    if (null != openGLControl)
                    {
                        sharpglControl.SetDraw(rs.GetPointclouds());
                    }
                   //;
                    Thread.Sleep(frameStep);
                }

            }
        }
        public RealsenseWidget(PictureBox picRgb, PictureBox picDepth,
            PictureBox picInfrared,OpenGLControl openGLControl)
        {
            this.picRgb = picRgb;
            this.picDepth = picDepth;
            this.picInfrared = picInfrared;
            this.openGLControl = openGLControl;
            InitCamera();
           if(null!=openGLControl)
                InitOpengl();
            displayThread = new Thread
                (new ThreadStart(LoopCapture));
            frameStep = 100;
        }



        /// <summary>
        /// 打开显示线程
        /// </summary>
        /// <param name="frameStep">每一帧中间暂定多久</param>
        public void StartDisplay()
        {     
            displayThread.Start();
        }

        /// <summary>
        /// 关闭显示线程，关闭相机
        /// </summary>
        public void StopDisplay()
        {
            displayThread.Abort();
            rs.Stop();
        }



    }
}
