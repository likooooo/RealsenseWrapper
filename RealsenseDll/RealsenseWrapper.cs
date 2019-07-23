using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.RealSense;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using PointcloudWrapper;

namespace RealsenseWrapper
{
    /**realsense库的封装
     * **/
   public class RealsenseControl
    {
        //当前的配置
        private Config currentConfig;
        private PipelineProfile profile;
        private Pipeline pipeline;
        //当前帧
        private FrameSet currentFrameset;
        public FrameSet CurrentFrameset
        {
            get
            {
                return currentFrameset;
            }
            set
            {
                if (currentFrameset != null)
                    currentFrameset.Dispose();
                currentFrameset = value;
            }
        }


        //定义Index =1 为左边的红外相机
        private readonly int infraredLeft = 1;
        private readonly int infraredRight = 2;

        //相机参数句柄
        private Module RGBModule;
        private Module DepthModule;
        private Module InfraredLeftModule;
        private Module InfraredRightModule;

        //彩色相机使能控制
        private bool colorFrameEnalbe;
        public bool ColorFrameEnable
        {
            get
            {
                return colorFrameEnalbe;
            }
            set
            {
                colorFrameEnalbe = value;

                if (RGBModule == null)
                    RGBModule = new Module(ModuleStream.Color);
                if (colorFrameEnalbe)
                    RGBModule.Enable(currentConfig);
                else
                    RGBModule.Disable(currentConfig);
            }
        }

        //深度相机使能控制
        private bool depthFrameEnalbe;
        public bool DepthFrameEnalbe
        {
            get
            {
                return depthFrameEnalbe;
            }
            set
            {
                depthFrameEnalbe = value;
                if (DepthModule == null)
                    DepthModule = new Module(ModuleStream.Depth);
                if (depthFrameEnalbe)
                    DepthModule.Enable(currentConfig);
                else
                    DepthModule.Disable(currentConfig);
            }
        }

        //红外相机使能控制
        private bool infraredLeftEnalbe;
        public bool InfraredLeftEnalbe
        {
            get
            {
                return infraredLeftEnalbe;
            }
            set
            {
                infraredLeftEnalbe = value;

                if (InfraredLeftModule == null)
                    InfraredLeftModule = new Module(ModuleStream.Infrared, infraredLeft);
                if (infraredLeftEnalbe)
                    InfraredLeftModule.Enable(currentConfig);
                else
                    InfraredLeftModule.Disable(currentConfig);
            }
        }
        private bool infraredRightEnalbe;
        public bool InfraredRightEnalbe
        {
            get
            {
                return infraredRightEnalbe;
            }
            set
            {
                if (InfraredRightModule == null)
                    InfraredRightModule = new Module(ModuleStream.Infrared, infraredRight);
                infraredRightEnalbe = value;
                if (infraredRightEnalbe)
                    InfraredRightModule.Enable(currentConfig);
                else
                    InfraredRightModule.Disable(currentConfig);
            }
        }


        /**构造函数
         * **/
        public RealsenseControl()
        {
            if (GetRealsenseDevice().Count <= 0)
                throw new Exception("没有找到realsense设备");
            currentConfig = new Config();
            pipeline = new Pipeline();
        }


        /********************************************************************
         * 下面是realsense控制部分
         *********************************************************************/


        /**打开realsense
         * **/
        public void Start()
        {
            profile = pipeline.Start();
        }
        public void StartWithCfg()
        {

            profile = pipeline.Start(currentConfig);
        }


        /**关闭realsense
         * **/
        public void Stop()
        {
            pipeline.Stop();
            profile.Dispose();
        }


        /**重启realsense，如果修改了相机使能，要生效需要执行ReStart()
         * **/
        public void ReStart()
        {
            Stop();
            Start();
        }


        /**更新一帧数据
         * **/
        public bool UpdateFrame()
        {
            FrameSet frames;
            for (int i = 0; i < 100; i++)
            {

                if (pipeline.TryWaitForFrames(out frames))
                {
                    CurrentFrameset = frames;
                    return true;
                }
            }
            return false;
        }


        /********************************************************************
         * 下面是数据处理部分，以Frameset处理为主
         *********************************************************************/


        /**获取当前连接的realsense设备集合
         * **/
        private DeviceList GetRealsenseDevice()
        {
            Context ctx = new Context();
            var list = ctx.QueryDevices();
            ctx.Dispose();
            return list;
        }


        /**获取深度单位
         * **/
        public float GetDepthScala()
        {
            if (profile != null)
                return 1 / profile.Device.Sensors[0].DepthScale;
            else
                return -1;
        }


        /**从当前帧数据获取图像数据
         * **/
        public RealsenseImage GetImage(ModuleStream moduleStream, int index = -1)
        {
            switch (moduleStream)
            {
                case ModuleStream.Color:
                    return GetRgbImage();
                case ModuleStream.Depth:
                    return GetDepthImage();
                case ModuleStream.Infrared:
                    return GetInfraredImage(index);
                default:
                    throw new Exception("预期之外的ModuleStream："+ moduleStream);
            }
        }
        public RealsenseImage GetRgbImage()
        {
            if (CurrentFrameset.ColorFrame == null)
                throw new Exception("在获取图像时，ColorFrame值为空,可能Depth未启用");
            RealsenseImage image = new RealsenseImage(
              CurrentFrameset.ColorFrame.Width,
              CurrentFrameset.ColorFrame.Height,
              CurrentFrameset.ColorFrame.Stride,
              CurrentFrameset.ColorFrame.BitsPerPixel);
            CurrentFrameset.ColorFrame.CopyTo<byte>(image.Data);
            return image;
        }
        public RealsenseImage GetDepthImage()
        {
            if (CurrentFrameset.DepthFrame == null)
                throw new Exception("在获取图像时，ColorFrame值为空,可能Depth未启用");
            RealsenseImage image = new RealsenseImage(
              CurrentFrameset.DepthFrame.Width,
              CurrentFrameset.DepthFrame.Height,
              CurrentFrameset.DepthFrame.Stride,
              CurrentFrameset.DepthFrame.BitsPerPixel);
            CurrentFrameset.DepthFrame.CopyTo<byte>(image.Data);
            return image;
        }
        public RealsenseImage GetInfraredImage(int index)
        {
            RealsenseImage image;
            switch (index)
            {
                case 1:
                    if (CurrentFrameset.InfraredFrame == null)
                        throw new Exception("在获取图像时，InfraredFrame值为空,可能Infrared未启用");
                    image = new RealsenseImage(
                      CurrentFrameset.InfraredFrame.Width,
                      CurrentFrameset.InfraredFrame.Height,
                      CurrentFrameset.InfraredFrame.Stride,
                      CurrentFrameset.InfraredFrame.BitsPerPixel);
                    CurrentFrameset.InfraredFrame.CopyTo<byte>(image.Data);
                    return image;
                case 2:
                    if (CurrentFrameset.FishEyeFrame == null)
                        throw new Exception("在获取图像时，FishEyeFrame值为空,可能Depth未启用");
                    image = new RealsenseImage(
                      CurrentFrameset.FishEyeFrame.Width,
                      CurrentFrameset.FishEyeFrame.Height,
                      CurrentFrameset.FishEyeFrame.Stride,
                      CurrentFrameset.FishEyeFrame.BitsPerPixel);
                    CurrentFrameset.FishEyeFrame.CopyTo<byte>(image.Data);
                    return image;
                default:
                    throw new Exception("当前Index定义为1或2");
            }
        }


        /**对当前帧进行对齐（在任何计算之前进行这一步操作）
         * **/
        public void Aligin(ModuleStream module= ModuleStream.Color)
        {
            
            using (Align align = new Align(Module.GetRsStream(module)))
            {
                CurrentFrameset = align.Process(CurrentFrameset);
            }
        }


        /**图像数据转换 RealsenseImage-》Bitmap
         * **/
        public Bitmap GetBitmap(RealsenseImage realsenseImage)
        {
            return realsenseImage.GetBitmap();
        }

        /**获取点云
         * **/
        public T[] GetPointclouds<T>()
        {
            T[] rsPoints;
            using (PointCloud pc = new PointCloud())
            using (var depth = CurrentFrameset.DepthFrame)
            {
                using (var points = pc.Process(depth).As<Points>())
                {
                    rsPoints = new T[points.Count];
                    points.CopyVertices(rsPoints);
                }
            }
            return rsPoints;
        }
        public RealsensePointcloud GetPointclouds()
        {
            RealsensePointcloud rsPoints;
            using (PointCloud pc = new PointCloud())
            using (var depth = CurrentFrameset.DepthFrame)
            {              
                using (var points = pc.Process(depth).As<Points>())
                {
                    //点云
                    Point[] point = new Point[points.Count];            
                    points.CopyVertices(point);                
                    rsPoints = new RealsensePointcloud(
                        point.Select(s=>s.x).ToArray(),
                          point.Select(s => s.y).ToArray(),
                            point.Select(s => s.z).ToArray());             
                }
            }
            return rsPoints;      
        }



        /**获取带纹理的点云
         * **/
        public (PointStruct[], TextureStruct[]) GetMapedPointclouds
            <PointStruct,TextureStruct>()
        {
            PointStruct[] rsPoints;
            TextureStruct[] textures;
            using (PointCloud pc = new PointCloud())
            {            
            using (var depth = CurrentFrameset.DepthFrame)
            {
                    using (var points = pc.Process(depth).As<Points>())
                    {
                        pc.MapTexture(CurrentFrameset.ColorFrame);

                        rsPoints = new PointStruct[points.Count];
                        textures = new TextureStruct[points.Count];
                        points.CopyVertices(rsPoints);
                        points.CopyTextureCoords(textures);
                    }
                }
            }
            return (rsPoints, textures);
        }
        public (RealsensePointcloud, Texture[]) GetMapedPointclouds()
        {
            RealsensePointcloud rsPoints;
            Texture[] textures;
            using (PointCloud pc = new PointCloud())
            using (var depth = CurrentFrameset.DepthFrame)
            {
                using (var points = pc.Process(depth).As<Points>())
                {
                    pc.MapTexture(CurrentFrameset.ColorFrame);
                    //点云
                    Point[] point = new Point[points.Count];
                    points.CopyVertices(point);
                    rsPoints = new RealsensePointcloud(
                        point.Select(s => s.x).ToArray(),
                          point.Select(s => s.y).ToArray(),
                            point.Select(s => s.z).ToArray());
                    textures = new Texture[points.Count];
                    points.CopyTextureCoords(textures);
                }
            }
            return (rsPoints, textures);
        }

    }


    struct Point
    {
        public float x, y, z;
    }
    public struct Texture
    {
        public float u, v;
    }



    /**realsense中提取的图片的数据结构
     * **/
    public class RealsenseImage
    {
        public int Width;
        public int Height;
        public int Stride;
        public int BitsPerPixel;
        public byte[] Data;


        /**构造函数
         * **/
        public RealsenseImage(int Width, int Height, int Stride, int BitsPerPixel)
        {
            this.Width = Width;
            this.Height = Height;
            this.Stride = Stride;
            this.BitsPerPixel = BitsPerPixel;
            Data = new byte[Stride * BitsPerPixel * Height];
        }


        /**数据转换RealsenseImage-》Bitmap
         * **/
        public Bitmap GetBitmap()
        {
            PixelFormat pixelFormat;
            switch (BitsPerPixel)
            {
                case 8:
                    pixelFormat = PixelFormat.Format8bppIndexed;
                    break;
                case 16:
                    pixelFormat = PixelFormat.Format16bppRgb565;
                    break;
                case 24:
                    pixelFormat = PixelFormat.Format24bppRgb;
                    break;
                default:
                    throw new Exception("RealsenseImage.GetBitmap()未定义BitsPerPixel的值："+ BitsPerPixel);
            }

            //创建一个bitmap对象
            Bitmap bmp = new Bitmap(Width, Height, pixelFormat);
          
            //通过bmp.LockBits()来复制值，这个方式效率高
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, Height),
             ImageLockMode.WriteOnly, pixelFormat);
            IntPtr iptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(Data, 0, iptr, Stride * Height);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }

    public class RealsensePointcloud : PointcloudF
    {
        public RealsensePointcloud(float[] x, float[] y, float[] z)
            : base(x, y, z)
        {

        }
    }
}
