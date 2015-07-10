using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ETS.Enums;
using Ets.Model.Common;
using ETS.Const;
using ETS.IO;
using ETS.Util;

namespace SuperManWebApi.Providers
{
    public class ImageHelper
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="httpPostedFile">待文件</param>
        /// <param name="orderId"></param>
        /// <param name="isReceipt">是否是上传小票</param>
        /// <param name="userType">用户类型   Business    Clienter</param>
        /// <returns></returns>
        public ImgInfo UploadImg(HttpPostedFile httpPostedFile, int orderId, bool isReceipt, UserType userType = UserType.Business)
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
            string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), SystemConst.OriginSize, Path.GetExtension(fileName));
            //原始的
            imgInfo.OriginFileName = rFileName;
            string saveDbFilePath;
            string saveDir = "";
            string basePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.GetPhysicalPath(isReceipt, userType);
            if (orderId > 0)
            {
                saveDir = orderId.ToString();
            }
            string fullFileDir = ETS.Util.ImageTools.CreateDirectory(basePath, saveDir, out saveDbFilePath);
            imgInfo.FullFileDir = fullFileDir;
            imgInfo.SaveDbFilePath = saveDbFilePath;
            if (fullFileDir == "0")
            {
                imgInfo.FailRemark = "创建目录失败";
                return imgInfo;
            }
            //保存原图，
            ///TODO记录图片大小
            var fullFilePath = Path.Combine(fullFileDir, rFileName);
            httpPostedFile.SaveAs(fullFilePath);
            //裁图
            var transformer = new ETS.Compress.FixedDimensionTransformerAttribute(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Width, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Height, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.MaxBytesLength / 1024);
            //保存到数据库的图片路径
            var destFullFileName = System.IO.Path.Combine(fullFileDir, fileName);
            transformer.Transform(fullFilePath, destFullFileName);

            var picUrl = saveDbFilePath + fileName;
            imgInfo.PicUrl = picUrl;
            return imgInfo;
        }

        /// <summary>
        /// 删除磁盘中的图片
        /// wc
        /// </summary>
        /// <param name="ticketUrl"></param>
        /// <returns></returns>
        public void DeleteTicket(string ticketUrl)
        {
            Regex regex = new Regex(@"(/\d{4}/\d{2}/\d{2}.*?)\.jpg", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matchCollection = regex.Matches(ticketUrl);
            string delPicDir = "1.jpg";
            foreach (Match match in matchCollection)
            {
                delPicDir = match.Value;
            }
            string ppath = ConfigSettings.Instance.FileUploadPath + "\\" + ConfigSettings.Instance.FileUploadFolderNameCustomerIcon;
            var delDir = ppath + delPicDir;
            var fileName = Path.GetFileName(delDir);
            int fileNameLastDot = fileName.LastIndexOf('.');
            //原图 
            string orginalFileName = string.Format("{0}{1}{2}", Path.GetDirectoryName(delDir) + "\\" + fileName.Substring(0, fileNameLastDot), SystemConst.OriginSize, Path.GetExtension(fileName));

            //删除磁盘中的裁图
            FileHelper.DeleteFile(delDir);
            //删除缩略图
            FileHelper.DeleteFile(orginalFileName);
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
