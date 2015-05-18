using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ETS.Const;

namespace SuperManWebApi.Providers
{
    public class ImageHelper
    {
        public ImgInfo UploadImg(HttpPostedFile httpPostedFile,int orderId)
        {
            ImgInfo imgInfo = new ImgInfo(); 
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(httpPostedFile.InputStream);
            }
            catch (Exception)
            {
                imgInfo.FailRemark = "无图片流";
                return imgInfo;
            } 
            var fileName = ETS.Util.ImageTools.GetFileName(Path.GetExtension(httpPostedFile.FileName)); 
            imgInfo.FileName = fileName; 
            int fileNameLastDot = fileName.LastIndexOf('.');
            //原图 
            string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), ImageConst.OriginSize, Path.GetExtension(fileName));
            //原始的
            imgInfo.OriginFileName = rFileName;
            string saveDbFilePath;
            string saveDir = "";
            if (orderId > 0) saveDir = orderId.ToString();
            string fullFileDir = ETS.Util.ImageTools.CreateDirectory(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, saveDir, out saveDbFilePath);
            imgInfo.FullFileDir = fullFileDir;
            imgInfo.SaveDbFilePath = saveDbFilePath;
            if (fullFileDir == "0")
            {
                imgInfo.FailRemark = "创建目录失败";
                return imgInfo;
            }
            //保存原图
            var fullFilePath = Path.Combine(fullFileDir, rFileName);  
            httpPostedFile.SaveAs(fullFilePath); 
            //裁图
            var transformer = new SuperManCore.FixedDimensionTransformerAttribute(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Width, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Height, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.MaxBytesLength / 1024);
            //保存到数据库的图片路径
            var destFullFileName = System.IO.Path.Combine(fullFileDir, fileName);
            transformer.Transform(fullFilePath, destFullFileName);

            var picUrl = saveDbFilePath + fileName;
            imgInfo.PicUrl = picUrl;
            return imgInfo;
        }
    }



    /// <summary>
    /// 图片信息
    /// </summary>
    public class ImgInfo
    {
        /// <summary>
        /// 文件名称，无后缀
        /// </summary>
        public string FileNameNoSuffix { get; set; }
        /// <summary>
        /// 文件名无后缀
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 原图名称，加_0_0 
        /// </summary>
        public string OriginFileName { get; set; }
        /// <summary>
        /// 目录结构
        /// </summary>
        public string SaveDbFilePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FullFileDir { get; set; }

        public string PicUrl { get; set; }

        public string FailRemark { get; set; }
    }
    
}
