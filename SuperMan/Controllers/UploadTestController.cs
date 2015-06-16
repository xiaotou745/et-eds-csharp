using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Clienter;
namespace SuperMan.Controllers
{
    [WebHandleError]
    public class UploadTestController : Controller
    { 
        public ActionResult Index()
        {
            return View();
        } 
        [HttpPost]
        public JsonResult Upload()
        {
            HttpPostedFileBase file = Request.Files[0]; 
            if (file != null)
            {
                if (file.ContentLength > 2097152)
                {
                    return Json(new ResultModel(false, "图片太大"), JsonRequestBehavior.AllowGet);  
                }
                string fileExt = Path.GetExtension(file.FileName).ToLower();
                if (string.IsNullOrEmpty(fileExt))
                {
                    return Json(new ResultModel(false, "格式不对"), JsonRequestBehavior.AllowGet);  
                }
                else
                {
                    bool allowUploadExt = false;
                    string allUploadExts = ".gif|.jpg|.jpeg|.png";
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
                        return Json(new ResultModel(false, "请上传正确的格式"), JsonRequestBehavior.AllowGet);  
                    }
                }

                var fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddhhmmss"), file.FileName);

                if (!System.IO.Directory.Exists(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath);
                }
                var fullFilePath = Path.Combine(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, fileName);
 
                file.SaveAs(fullFilePath);

                var transformer = new ETS.Compress.FixedDimensionTransformerAttribute(CustomerIconUploader.Instance.Width, CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);

                var destFileName = string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random().Next(1000), Path.GetExtension(file.FileName));
                var destFullFileName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, destFileName);
                transformer.Transform(fullFilePath, destFullFileName); 
                 
                var picUrl = System.IO.Path.GetFileName(destFullFileName);
                return Json(new ResultModel(true, picUrl + "|" + file.FileName), JsonRequestBehavior.AllowGet);  
            }

            return Json(new ResultModel(false, ""), JsonRequestBehavior.AllowGet);
        } 
    }
}