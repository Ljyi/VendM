using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace VendM.Core.Utils.Images
{
    /// <summary>  
    /// 水印图片的操作管理 Design by Gary Gong From Demetersoft.com  
    /// </summary>  
    public static class WaterImageManage
    {
        private static KeyValuePair<Font, SizeF> GetFontInfo(string fontStyle, int fontSize, string waterWords,
            Graphics grPhoto, int phWidth)
        {
            if (fontSize == 0)
            {
                //根据图片的大小我们来确定添加上去的文字的大小  
                //在这里我们定义一个数组来确定  
                var sizes = new[] { 16, 14, 12, 10, 8, 6, 4 };

                Font crFont = null; //字体  
                //矩形的宽度和高度，SizeF有三个属性，分别为Height高，width宽，IsEmpty是否为空  
                var crSize = new SizeF();

                //利用一个循环语句来选择我们要添加文字的型号  
                //直到它的长度比图片的宽度小  
                for (var i = 0; i < 7; i++)
                {
                    crFont = new Font(fontStyle, sizes[i], FontStyle.Bold);
                    //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。  
                    crSize = grPhoto.MeasureString(waterWords, crFont);

                    // ushort 关键字表示一种整数数据类型  
                    if ((ushort)crSize.Width < (ushort)phWidth)
                        break;
                }
                return new KeyValuePair<Font, SizeF>(crFont, crSize);
            }
            else
            {
                var crFont = new Font(fontStyle, fontSize, FontStyle.Bold);
                var crSize = grPhoto.MeasureString(waterWords, crFont);
                return new KeyValuePair<Font, SizeF>(crFont, crSize);
            }
        }

        /// <summary>  
        /// 添加图片水印  
        /// </summary>  
        /// <param name="filestream">源图片文件名</param>  
        /// <param name="waterImage">水印图片文件名</param>  
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>  
        /// <param name="position">位置</param>
        /// <param name="fileName"></param>
        /// <returns>返回生成于指定文件夹下的水印文件名</returns>  
        public static byte[] DrawImage(byte[] filestream,
            string waterImage,
            float alpha,
            WatermarkPosition position, string fileName)
        {
            if (filestream == null || waterImage == string.Empty || alpha == 0.0f)
                return null;

            var imgPhoto = Image.FromStream(new MemoryStream(filestream));

            // 确定其长宽   
            var phWidth = imgPhoto.Width;
            var phHeight = imgPhoto.Height;

            var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。  

            // 设定分辨率  
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            var grPhoto = Graphics.FromImage(bmPhoto); // 定义一个绘图画面用来装载位图  

            Image imgWatermark = new Bitmap(waterImage); //同样，由于水印是图片，我们也需要定义一个Image来装载它  


            // 获取水印图片的高度和宽度   
            var wmWidth = imgWatermark.Width;
            var wmHeight = imgWatermark.Height;

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。  
            // 成员名称   说明   
            // AntiAlias      指定消除锯齿的呈现。    
            // Default        指定不消除锯齿。    
            // HighQuality  指定高质量、低速度呈现。    
            // HighSpeed   指定高速度、低质量呈现。    
            // Invalid        指定一个无效模式。    
            // None          指定不消除锯齿。   
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            // 第一次描绘，将我们的底图描绘在绘图画面上  
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, phWidth, phHeight), 0, 0, phWidth, phHeight,
                GraphicsUnit.Pixel);

            var bmWatermark = new Bitmap(bmPhoto); // 与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率  
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            var grWatermark = Graphics.FromImage(bmWatermark); // 继续，将水印图片装载到一个绘图画面grWatermark  

            var imageAttributes = new ImageAttributes(); //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。  


            var colorMap = new ColorMap
            {
                OldColor = Color.FromArgb(255, 0, 255, 0), //我的水印图被定义成拥有绿色背景色的图片被替换成透明  
                NewColor = Color.FromArgb(0, 0, 0, 0)
            }; //Colormap: 定义转换颜色的映射  


            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements =
            {
                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f}, // red红色  
                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f}, //green绿色  
                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f}, //blue蓝色         
                new float[] {0.0f, 0.0f, 0.0f, alpha, 0.0f}, //透明度       
                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
            }; //  

            //  ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。  
            //  ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。  
            var wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            //上面设置完颜色，下面开始设置位置   
            int xPosOfWm;
            int yPosOfWm;

            switch (position)
            {
                case WatermarkPosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case WatermarkPosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case WatermarkPosition.LeftBottom:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case WatermarkPosition.LeftTop:
                    xPosOfWm = 10;
                    yPosOfWm = 10;
                    break;
                case WatermarkPosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = 10;
                    break;
                case WatermarkPosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case WatermarkPosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = 10;
                    break;
                default:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }

            // 第二次绘图，把水印印上去  
            grWatermark.DrawImage(imgWatermark, new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight, GraphicsUnit.Pixel, imageAttributes);

            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();
            // 保存文件到服务器的文件夹里面  
            //imgPhoto.Save(targetImage, GetImageFormat(fileSourceExtension));
            var bytes = ImageToBytes(imgPhoto, GetImageFormat(fileName));
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filestream"></param>
        /// <param name="waterWords"></param>
        /// <param name="alpha"></param>
        /// <param name="position"></param>
        /// <param name="fileName"></param>
        /// <param name="fontStyle"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] DrawWords(byte[] filestream, string waterWords, float alpha, WatermarkPosition position, string fileName, string fontStyle = "arial", int size = 0)
        {
            if (filestream == null || string.IsNullOrEmpty(waterWords) || alpha == 0.0f) return null;

            var imgPhoto = Image.FromStream(new MemoryStream(filestream));
            //获取图片的宽和高  
            var phWidth = imgPhoto.Width;
            var phHeight = imgPhoto.Height;

            //建立一个bitmap，和我们需要加水印的图片一样大小  
            var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //SetResolution：设置此 Bitmap 的分辨率  
            //这里直接将我们需要添加水印的图片的分辨率赋给了bitmap  
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //Graphics：封装一个 GDI+ 绘图图面。  
            var grPhoto = Graphics.FromImage(bmPhoto);
            //设置图形的品质  
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //将我们要添加水印的图片按照原始大小描绘（复制）到图形中  
            grPhoto.DrawImage(imgPhoto, //   要添加水印的图片  
                new Rectangle(0, 0, phWidth, phHeight), //  根据要添加的水印图片的宽和高  
                0, //  X方向从0点开始描绘  
                0, // Y方向   
                phWidth, //  X方向描绘长度  
                phHeight, //  Y方向描绘长度  
                GraphicsUnit.Pixel); // 描绘的单位，这里用的是像素  

            var fontSizeKvp = GetFontInfo(fontStyle, size, waterWords, grPhoto, phWidth);
            var crFont = fontSizeKvp.Key;
            var crSize = fontSizeKvp.Value;

            //定义在图片上文字的位置  
            var wmHeight = crSize.Height;
            var wmWidth = crSize.Width;

            float xPosOfWm;
            float yPosOfWm;

            switch (position)
            {
                case WatermarkPosition.BottomMiddle:
                    xPosOfWm = (float)phWidth / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case WatermarkPosition.Center:
                    xPosOfWm = (float)phWidth / 2;
                    yPosOfWm = (float)phHeight / 2;
                    break;
                case WatermarkPosition.LeftBottom:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case WatermarkPosition.LeftTop:
                    xPosOfWm = wmWidth / 2;
                    yPosOfWm = wmHeight / 2;
                    break;
                case WatermarkPosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = wmHeight;
                    break;
                case WatermarkPosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case WatermarkPosition.TopMiddle:
                    xPosOfWm = (float)phWidth / 2;
                    yPosOfWm = wmWidth;
                    break;
                default:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }

            //封装文本布局信息（如对齐、文字方向和 Tab 停靠位），显示操作（如省略号插入和国家标准 (National) 数字替换）和 OpenType 功能。  
            var strFormat = new StringFormat { Alignment = StringAlignment.Center };

            //定义需要印的文字居中对齐  

            //SolidBrush:定义单色画笔。画笔用于填充图形形状，如矩形、椭圆、扇形、多边形和封闭路径。  
            //这个画笔为描绘阴影的画笔，呈灰色  
            var mAlpha = Convert.ToInt32(256 * alpha);
            var semiTransBrush2 = new SolidBrush(Color.FromArgb(mAlpha, 0, 0, 0));

            //描绘文字信息，这个图层向右和向下偏移一个像素，表示阴影效果  
            //DrawString 在指定矩形并且用指定的 Brush 和 Font 对象绘制指定的文本字符串。  
            grPhoto.DrawString(waterWords, //string of text  
                crFont, //font  
                semiTransBrush2, //Brush  
                new PointF(xPosOfWm + 1, yPosOfWm + 1), //Position  
                strFormat);

            //从四个 ARGB 分量（alpha、红色、绿色和蓝色）值创建 Color 结构，这里设置透明度为153  
            //这个画笔为描绘正式文字的笔刷，呈白色  
            var semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //第二次绘制这个图形，建立在第一次描绘的基础上  
            grPhoto.DrawString(waterWords, //string of text  
                crFont, //font  
                semiTransBrush, //Brush  
                new PointF(xPosOfWm, yPosOfWm), //Position  
                strFormat);

            //imgPhoto是我们建立的用来装载最终图形的Image对象  
            //bmPhoto是我们用来制作图形的容器，为Bitmap对象  
            imgPhoto = bmPhoto;
            //释放资源，将定义的Graphics实例grPhoto释放，grPhoto功德圆满  
            grPhoto.Dispose();

            var bytes = ImageToBytes(imgPhoto, GetImageFormat(fileName));
            imgPhoto.Dispose();
            return bytes;
        }

        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private static byte[] ImageToBytes(Image image, ImageFormat format)
        {
            if (format == null)
                format = ImageFormat.MemoryBmp;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);

                var buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin); //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        private static ImageFormat GetImageFormat(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return ImageFormat.MemoryBmp;

            var fileExtension = Path.GetExtension(fileName).ToLower();
            switch (fileExtension)
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".emf":
                    return ImageFormat.Emf;
                case ".exif":
                    return ImageFormat.Exif;
                case ".gif":
                    return ImageFormat.Gif;
                case ".ico":
                    return ImageFormat.Icon;
                case ".png":
                    return ImageFormat.Png;
                case ".tif":
                    return ImageFormat.Tiff;
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".wmf":
                    return ImageFormat.Wmf;
                default:
                    return ImageFormat.Jpeg;
            }
        }
    }
}
