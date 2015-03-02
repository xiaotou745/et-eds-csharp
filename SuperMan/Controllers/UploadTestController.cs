using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class UploadTestController : Controller
    { 
        public ActionResult Upload()
        {
            HttpPostedFileBase file = Request.Files["fileUpload"];
            if (file != null)
            {
                if (file.ContentLength > 2097152)
                {
                    //MessageBoxAndReturn("文件超过大小限制！");
                }
                string fileExt = Path.GetExtension(file.FileName).ToLower();
                if (string.IsNullOrEmpty(fileExt))
                {
                    //("上传的文件格式有问题！");
                }
                else
                {
                    bool allowUploadExt = false;
                    string allUploadExts = ".gif|.jpg|.jpeg";
                    string[] exps = allUploadExts.Split('|');
                    foreach (string exp in exps)
                    {
                        if (exp.ToLower() == fileExt)
                        {
                            allowUploadExt = true;
                            break;
                        }
                    }
                    if (!allowUploadExt)
                    {
                        //MessageBoxAndReturn(string.Format("您的上传文件格式错误({0})！", allUploadExts.Replace(".", "")));
                    }
                } 

                //保存文件
                string saveDoc = "/uploads";
                string rdFileName = getRandomFileName();
                string newFileName = rdFileName + fileExt;
                if (!Directory.Exists(Path.GetDirectoryName(Server.MapPath(saveDoc))))
                {
                    Directory.CreateDirectory(Server.MapPath(saveDoc));
                }
                // 文件上传  
                file.SaveAs(Server.MapPath(saveDoc + newFileName)); 
            }
            return View();
        }

        /// <summary>
        /// 自动命名,返回文件名：年_月_日_时_分_秒_随机数
        /// </summary>
        /// <param name="Ext">扩展名</param>
        /// <returns>返回文件名如：2005_10_1_12_10_10_2345</returns>
        public static string getRandomFileName()
        {
            Random rdom = new Random();
            DateTime dtime = DateTime.Now;
            string Filename = dtime.Year + "_" + dtime.Month + "_" + dtime.Day + "_" + dtime.Hour + "_" + dtime.Minute + "_" + dtime.Second + "_" + rdom.Next(10000);
            return Filename;
        }
    }
}