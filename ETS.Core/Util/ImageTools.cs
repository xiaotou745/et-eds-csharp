using System;
using System.Collections.Generic;
using System.IO;
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
        public static string GetFileName(string fileMark="", string fileExt = ".jpg")
        {
            //Random rdom = new Random();
            //DateTime dtime = DateTime.Now; 
            //string filename = string.Format("{0}_{1}_{2}{3}", dtime.ToString("yyyyMMddhhmmssfff"), rdom.Next(10000), fileMark, fileExt); 
            string fileName = Path.GetFileName(ETS.Util.Helper.CreateImageName(fileExt));

            return fileName;
        }
        /// <summary>
        /// 创建图片上传目录
        /// </summary>
        /// <param name="uploadPath">图片上传的物理路径</param>
        /// <param name="saveDir">保存到哪个文件夹下,没有的话就传空字符串</param>
        /// <returns></returns>
        public static string CreateDirectory(string uploadPath,string saveDir,out string virtualPath)
        {
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(saveDir))
            {
                virtualPath =  DateTime.Now.ToString("/yyyy/MM/dd/HH/");
            }
            else
            {
                virtualPath = DateTime.Now.ToString("/yyyy/MM/dd/HH/") + saveDir + "/";
            }
            
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
