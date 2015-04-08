using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Util
{
    public class ImageTools
    {
        /// <summary>
        /// 生成一个图片的文件名
        /// </summary>
        /// <param name="fileExt">图片后缀</param>
        /// <param name="fileMark">图片表示,B端还是C端</param>
        /// <returns></returns>
        public static string GetFileName(string fileMark="E", string fileExt = ".jpg")
        {
            Random rdom = new Random();
            DateTime dtime = DateTime.Now; 
            string filename = string.Format("{0}_{1}_{2}{3}", dtime.ToString("yyyyMMddhhmmssfff"), rdom.Next(10000), fileMark, fileExt);
            return filename;
        }
        /// <summary>
        /// 创建图片上传目录
        /// </summary>
        /// <param name="uploadPath">图片上传的物理路径</param>
        /// <returns></returns>
        public static string CreateDirectory(string uploadPath,out string virtualPath )
        {
            DateTime dateTime = DateTime.Now;
            virtualPath = string.Format("\\{1}\\{2}\\{3}\\", dateTime.Year, dateTime.Month, dateTime.Day);
            string fileUploadDir = string.Format("{0}{1}", uploadPath, virtualPath);
            try
            {
                if (!System.IO.Directory.Exists(fileUploadDir))
                {
                    System.IO.Directory.CreateDirectory(fileUploadDir);
                }
                return fileUploadDir;  //创建成功
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("上传图片失败：", new { ex = ex });
                return "0";
            }
        }
    }
}
