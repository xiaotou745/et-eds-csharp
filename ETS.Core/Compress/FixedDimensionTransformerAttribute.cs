using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Compress
{
    public class FixedDimensionTransformerAttribute : Attribute, IImageFileTransformer
    {
        private int _width;
        private int _height;
        private int _maxByteLength;

        public FixedDimensionTransformerAttribute(int width, int height, int maxSizeInKb)
        {
            _width = width;
            _height = height;
            _maxByteLength = maxSizeInKb * 1024;
        }

        public Size Transform(string srcPath, string destPath)
        {
            var quality = 100;
            Size size = new Size();
            while (quality > 10)
            {
                size = ImageSizeTransformManager.Instance.ResizeImageToSize(srcPath, destPath, _width, _height, quality);
                var length = new FileInfo(destPath).Length;
                if (length > _maxByteLength)
                {
                    quality -= 5;
                    continue;
                }
                return size;
            }
            return size;
        }
    }
}
