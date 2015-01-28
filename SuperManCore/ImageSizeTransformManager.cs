using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore
{
    internal class ImageSizeTransformManager
    {
        public static readonly ImageSizeTransformManager Instance = new ImageSizeTransformManager();
        private ImageSizeTransformManager()
        {

        }

        /// <summary>
        /// 等比 压缩到指定宽
        /// </summary>
        public Size ResizeImageToWidth(string srcPath, string destPath, int destWidth, int quality = 100)
        {
            var srcImage = Image.FromFile(srcPath);
            var srcWidth = srcImage.Width;
            var srcHeight = srcImage.Height;

            double factor = (double)destWidth / srcWidth;
            var destHeight = (int)(srcHeight * factor);

            return ResizeImage(srcImage, destPath, destWidth, destHeight, quality);
        }

        /// <summary>
        /// 等比 压缩到指定宽，但是高度不能小于最小高度，否则从中间同比截取
        /// </summary>
        public Size ResizeImageToWidth(string srcPath, string destPath, int destWidth, int minHeight, int quality = 100)
        {
            var srcImage = Image.FromFile(srcPath);
            var srcWidth = srcImage.Width;
            var srcHeight = srcImage.Height;

            double factor = (double)destWidth / srcWidth;
            var destHeight = (int)(srcHeight * factor);
            if (destHeight <= minHeight)
            {
                destHeight = minHeight;
                factor = (double)destHeight / srcHeight;
            }

            var validWidth = (int)(destWidth / factor);
            var redundantWidth = srcWidth - validWidth;

            var srcRect = new Rectangle(redundantWidth / 2, 0, validWidth, srcHeight);
            return ResizeImage(srcImage, destPath, destWidth, destHeight, srcRect, quality);
        }

        /// <summary>
        /// 等比 压缩到指定高
        /// </summary>
        public Size ResizeImageToHeight(string srcPath, string destPath, int destHeight, int quality = 100)
        {
            var srcImage = Image.FromFile(srcPath);
            var srcWidth = srcImage.Width;
            var srcHeight = srcImage.Height;

            double factor = (double)destHeight / srcHeight;
            var destWidth = (int)(srcWidth * factor);

            return ResizeImage(srcImage, destPath, destWidth, destHeight, quality);
        }

        /// <summary>
        /// 等比 压缩到指定宽和高, 如果所需比例和原图不一致, 从中间截取
        /// </summary>
        public Size ResizeImageToSize(string srcPath, string destPath, int destWidth, int destHeight, int quality = 100)
        {
            var srcImage = Image.FromFile(srcPath);
            var srcWidth = srcImage.Width;
            var srcHeight = srcImage.Height;

            double factorWidth = (double)destWidth / srcWidth;
            double factorHeight = (double)destHeight / srcHeight;
            var invalidFactor = Math.Max(factorWidth, factorHeight);
            Rectangle srcRect;
            if (factorWidth > factorHeight)
            {
                var validHeight = (int)(destHeight / invalidFactor);
                var redundantHeight = srcHeight - validHeight;
                srcRect = new Rectangle(0, redundantHeight / 2, srcWidth, validHeight);
            }
            else
            {
                var validWidth = (int)(destWidth / invalidFactor);
                var redundantWidth = srcWidth - validWidth;
                srcRect = new Rectangle(redundantWidth / 2, 0, validWidth, srcHeight);
            }

            return ResizeImage(srcImage, destPath, destWidth, destHeight, srcRect, quality);
        }

        /// <summary>
        /// 等比压缩, 宽和高都不超过指定的宽和高。如果原图比较小，大小和原图保持一致。
        /// </summary>
        public Size ResizeImageWithinDimension(string srcPath, string destPath, int maxWidth, int maxHeight)
        {
            var srcImage = Image.FromFile(srcPath);
            var srcWidth = srcImage.Width;
            var srcHeight = srcImage.Height;

            var srcRect = new Rectangle(0, 0, srcWidth, srcHeight);

            double factorWidth = (double)maxWidth / srcWidth;
            double factorHeight = (double)maxHeight / srcHeight;
            var invalidFactor = Math.Min(factorWidth, factorHeight);
            if (invalidFactor >= 1)
            {
                invalidFactor = 1;
            }
            var destWidth = (int)(invalidFactor * srcWidth);
            var destHeight = (int)(invalidFactor * srcHeight);

            return ResizeImage(srcImage, destPath, destWidth, destHeight, srcRect, 100);
        }

        /// <summary>
        /// 压缩到指定宽和高, 如果比例不一样，会变形。
        /// </summary>
        public Size ResizeImage(string srcPath, string destPath, int destWidth, int destHeight)
        {
            var srcImage = Image.FromFile(srcPath);
            return ResizeImage(srcImage, destPath, destWidth, destHeight);
        }

        private Size ResizeImage(Image srcImage, string destPath, int destWidth, int destHeight, int quality = 100)
        {
            var srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);
            return ResizeImage(srcImage, destPath, destWidth, destHeight, srcRect, quality);
        }

        private Size ResizeImage(Image srcImage, string destPath, int destWidth, int destHeight, Rectangle srcRect, int quality)
        {
            var destSize = new Size(destWidth, destHeight);
            using (var destBitmap = new Bitmap(destWidth, destHeight))
            {
                var g = Graphics.FromImage(destBitmap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.White);
                var destRect = new Rectangle(Point.Empty, destSize);
                g.DrawImage(srcImage, destRect, srcRect, GraphicsUnit.Pixel);

                var parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
                var encoders = ImageCodecInfo.GetImageEncoders();
                var encoder = encoders.Where(i => i.FormatID == srcImage.RawFormat.Guid).FirstOrDefault();
                destBitmap.Save(destPath, encoder, parameters);
            }
            GC.Collect();
            return destSize;
        }
    }
}
