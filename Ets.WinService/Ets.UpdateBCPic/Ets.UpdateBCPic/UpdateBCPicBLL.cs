using Common.Logging;
using Ets.Dao.Business;
using Ets.Dao.Clienter;
using Ets.Dao.Order;
using Ets.Dao.User;
using ETS.IO;
using Ets.Model.Common;
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Service.Provider.Clienter;
using ETS;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace Ets.UpdateBCPic
{
    /// <summary>
    /// 更新商户和骑士图片业务  add by pengyi 20150709
    /// </summary>
    public class UpdateBCPicBLL// : Quartz.IJob
    {
        //使用Common.Logging.dll日志接口实现日志记录        
        private ILog logger = LogManager.GetCurrentClassLogger();
        private static bool threadSafe = true;//线程安全
        private static readonly int PAGET_SIZE = int.Parse(ConfigurationManager.AppSettings["PageSize"]);

        //private static readonly string BusinessBasePath = string.Format("{0}/{1}/",
        //    Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, ConfigSettings.Instance.FileUploadFolderNameBusiness);
        //private static readonly string ClienterBasePath = string.Format("{0}/{1}/",
        //    Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, ConfigSettings.Instance.FileUploadFolderNameClienter);

        private static readonly string BusinessBasePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.BusinessPicPhysicalPath;
        private static readonly string ClienterBasePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.ClienterPicPhysicalPath;

        #region IJob 成员

        public void Execute()//(Quartz.IJobExecutionContext context)
        {
            if (!threadSafe)
            {
                return;
            }
            threadSafe = false;
            try
            {
                LogHelper.LogWriter("执行啦:" + DateTime.Now);

                ClienterDao clienterDao = new ClienterDao();
                //创建基础路径
                if (!System.IO.Directory.Exists(BusinessBasePath))
                {
                    System.IO.Directory.CreateDirectory(BusinessBasePath);
                }
                if (!System.IO.Directory.Exists(ClienterBasePath))
                {
                    System.IO.Directory.CreateDirectory(ClienterBasePath);
                }

                //更新商户图片
                UpdateBusinessPic();
                LogHelper.LogWriter("更新商户图片完成");

                //更新骑士图片
                UpdateClienterPic();
                LogHelper.LogWriter("更新骑士图片完成");
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex);
            }
            finally
            {
                threadSafe = true;
            }
        }


        #endregion

        #region 更新商户图片

        private void UpdateBusinessPic()
        {
            BusinessDao businessDao = new BusinessDao();
            var count = businessDao.GetBusinsessCount();
            var pageCount = count / PAGET_SIZE + 1;
            for (int i = 0; i < pageCount; i++)
            {
                var pageRet = new PagingResult(i+1, PAGET_SIZE);
                CopyBusinessPics(businessDao, pageRet);
            }
        }

        private void CopyBusinessPics(BusinessDao businessDao, PagingResult pageRet)
        {
            var pageinfo = businessDao.GetBusinessPics<BusinessPicModel>(pageRet);
            foreach (var record in pageinfo.Records)
            {
                if (!string.IsNullOrEmpty(record.CheckPicUrl))
                {
                    var sourcePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath + record.CheckPicUrl;
                    var destPath = BusinessBasePath + record.CheckPicUrl;
                    FileHelper.Copy(sourcePath, destPath);
                    LogHelper.LogWriter(string.Format("copy business:{0}  {1} to {2}", record.Id, sourcePath, destPath));
                }
                if (!string.IsNullOrEmpty(record.BusinessLicensePic))
                {
                    var sourcePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath + record.BusinessLicensePic;
                    var destPath = BusinessBasePath + record.BusinessLicensePic;
                    FileHelper.Copy(sourcePath, destPath);
                    LogHelper.LogWriter(string.Format("copy business:{0}  {1} to {2}", record.Id, sourcePath, destPath));
                }
            }
        }

        #endregion

        #region 更新骑士图片

        private void UpdateClienterPic()
        {
            ClienterDao clienterDao = new ClienterDao();
            var count = clienterDao.GetClienterCount();
            var pageCount = count / PAGET_SIZE + 1;
            for (int i = 0; i < pageCount; i++)
            {
                var pageRet = new PagingResult(i, PAGET_SIZE);
                CopyClienterPics(clienterDao, pageRet);
            }
        }

        private void CopyClienterPics(ClienterDao clienterDao, PagingResult pageRet)
        {
            var pageinfo = clienterDao.GetClienterPics<ClienterPicModel>(pageRet);
            foreach (var record in pageinfo.Records)
            {
                if (!string.IsNullOrEmpty(record.PicUrl))
                {
                    var sourcePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath + record.PicUrl;
                    var destPath = ClienterBasePath + record.PicUrl;
                    FileHelper.Copy(sourcePath, destPath);
                    LogHelper.LogWriter(string.Format("copy clienter:{0}  {1} to {2}", record.Id, sourcePath, destPath));
                }
                if (!string.IsNullOrEmpty(record.PicWithHandUrl))
                {
                    var sourcePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath + record.PicWithHandUrl;
                    var destPath = ClienterBasePath + record.PicWithHandUrl;
                    FileHelper.Copy(sourcePath, destPath);
                    LogHelper.LogWriter(string.Format("copy clienter:{0}  {1} to {2}", record.Id, sourcePath, destPath));
                }
            }
        }

        #endregion
    }
}
