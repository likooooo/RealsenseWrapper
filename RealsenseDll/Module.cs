using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealsenseWrapper
{
    public enum ModuleStream
    {
        Depth = 1,
        Color = 2,
        Infrared = 3,
    }

    /**传感器可用格式枚举
     * **/
    public enum ModuleFormat
    {
        //深度格式
        Z16 = 1,
        //彩色格式
        Yuyv = 4,
        Rgb8 = 5,
        Bgr8 = 6,
        Rgba8 = 7,
        Bgra8 = 8,        //Raw16 = 12,
        //红外格式
        Y8 = 9,
        Y16 = 10,
    }


    /**传感器采集的图像大小
     * **/
    public struct ImagePart
    {
        public int width, height;


        public ImagePart(int width,int height)
        {
            this.width = width;
            this.height = height;
        }
    }


    /**传感器初始化
     * **/
    class Module
    {
        private ModuleStream moduleType;
        public ModuleStream ModuleType
        {
            get
            {
                return moduleType;
            }
        }

        private ImagePart imagePart = new ImagePart(1280, 720);
        public ImagePart ImagePart
        {
            get
            {
                return imagePart;
            }
        }

        private int frameRate = 30;
        public int FrameRate
        {
            get
            {
                return frameRate;
            }
        }

        private ModuleFormat format;      
        public ModuleFormat Format
        {
            get
            {
                return format;
            }
        }

        //如果是红外此处需要填写index
        private int index;


        /**构造函数
         * ModuleType : 模块类型
         * 参数都是默认值(依据来源于 Intel RealSense Viewer)
         * **/
        public Module(ModuleStream module)
        {
            if (ModuleStream.Infrared == module)
            {
                throw new Exception("构建红外模组需要使用带Index的构造函数！");
            }
            moduleType = module;
            switch (module)
            {
                case ModuleStream.Color:
                    DefaultRGBCamera();
                    break;
                case ModuleStream.Depth:
                    DefaultDepthCamera();
                    break;

            }
        }
        /**index默认为-1，
         * **/
        public Module(ModuleStream module, int index)
        {
            this.index = -1;
            switch (module)
            {
                case ModuleStream.Color:
                    DefaultRGBCamera();
                    break;
                case ModuleStream.Depth:
                    DefaultDepthCamera();
                    break;
                case ModuleStream.Infrared:
                    DefaultInfraredCamera();
                    this.index = index;
                    break;
            }
        }
        /**如果不想要默认的format，则用这个构造函数
         * **/
        public Module(ModuleStream module, ModuleFormat format)
        {
            moduleType = module;
            this.format = format;
        }


        /**将当前配置更新到对应的Config
         * **/
        public void Enable(Intel.RealSense.Config config)
        {            
            //如果是红外，EnableStream需要定义 index,其他情况下不需要Index
            if (moduleType == ModuleStream.Infrared)
            {
                config.EnableStream(
                     GetRsStream(ModuleType),
                     index,
                     imagePart.width,
                     imagePart.height,
                     GetRs2Format(Format),
                     FrameRate);

            }
            else
            {
                config.EnableStream(
                          GetRsStream(ModuleType),
                          imagePart.width,
                          imagePart.height,
                          GetRs2Format(Format),
                          FrameRate);
            }

        }


        /**将当前配置禁止到对应的Config
        * **/
        public void Disable(Intel.RealSense.Config config)
        {
            //如果是红外，DisableStream需要定义 index,其他情况下不需要Index
            if (moduleType == ModuleStream.Infrared)
            {
                config.DisableStream(GetRsStream(ModuleType),index);
            }
            else
            {
                config.DisableStream(GetRsStream(ModuleType));
            }

        }


        /***自己定义的枚举与Realsense自带枚举的转换
         * ModuleStream-》Intel.RealSense.Stream
         * **/
        public static Intel.RealSense.Stream GetRsStream(ModuleStream module)
        {
            Intel.RealSense.Stream rsStream= (Intel.RealSense.Stream)module;
            return rsStream;
        }
        /**自己定义的枚举与Realsense自带枚举的转换
         * ModuleFormat-》Intel.RealSense.Format
         * **/
        public static Intel.RealSense.Format GetRs2Format(ModuleFormat format)
        {
            Intel.RealSense.Format rsFormat= (Intel.RealSense.Format)format; 
            return rsFormat;
        }


        /**默认相机配置
         * **/
        private void DefaultRGBCamera()
        {
            //彩色图像如果定义成rgb8的话，
            //在 RealsenseImage.GetBitmap()中
            //返回的24位Bmp图像颜色是反的
            format = ModuleFormat.Bgr8;
        }
        private void DefaultDepthCamera()
        {
            format = ModuleFormat.Z16;
        }
        private void DefaultInfraredCamera()
        {
            format = ModuleFormat.Y8;
        }
    }
}
