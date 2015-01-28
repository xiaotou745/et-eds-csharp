using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SuperManCore
{
    /// <summary>
    /// 固定尺寸的Uploader
    /// </summary>
    public abstract class FixedDimensionUploader : FileUploaderBase
    {
        public abstract int Width { get; }

        public abstract int Height { get; }

        public override bool Validate(string fileName, Stream stream, out string reason)
        {
            reason = string.Empty;

            if (!AllowedImageTypes.Any(i => i == Path.GetExtension(fileName).ToLower()))
            {
                reason = string.Format("上传失败！请选择{0}类型的文件", string.Join(",", AllowedImageTypes));
                return false;
            }

            using (Image image = Image.FromStream(stream))
            {
                if (image.Width != Width)
                {
                    reason = string.Format("上传失败！图片宽度必须等于{0}", Width);
                    return false;
                }

                if (image.Height != Height)
                {
                    reason = string.Format("上传失败！图片高度必须等于{0}", Height);
                    return false;
                }
            }

            if (stream.Length > MaxBytesLength)
            {
                reason = string.Format("上传失败！图片大小必须小于{0}k", MaxBytesLength / 1024);
                return false;
            }
            return true;
        }
    }
}
