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
                return null;
            }
            
            var fileName = ETS.Util.ImageTools.GetFileName(Path.GetExtension(httpPostedFile.FileName));

            imgInfo.FileName = fileName;

            int fileNameLastDot = fileName.LastIndexOf('.');
            //原图 
            string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), ImageConst.OriginSize, Path.GetExtension(fileName));

            string saveDbFilePath;

            string fullFileDir = ETS.Util.ImageTools.CreateDirectory(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, orderId.ToString(), out saveDbFilePath);

            if (fullFileDir == "0")
            {
                return null;
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


            return imgInfo;
        }
    }
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

        public string SaveDbFilePath { get; set; }

        public string FullFileDir { get; set; }

        public string PicUrl { get; set; }

        public string FailRemark { get; set; }
    }
    
}
