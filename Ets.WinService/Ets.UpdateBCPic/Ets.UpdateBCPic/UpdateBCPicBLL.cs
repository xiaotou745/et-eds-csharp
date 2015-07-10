using System.IO;
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
using NLog;

namespace Ets.UpdateBCPic
{
    /// <summary>
    /// 更新商户和骑士图片业务  add by pengyi 20150709
    /// </summary>
    public class UpdateBCPicBLL
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly string BusinessBasePath =
            Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.BusinessPicPhysicalPath;
        private static readonly string ClienterBasePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.ClienterPicPhysicalPath;


        public void Execute()
        {
       
            try
            {
                logger.Info("执行啦:" + DateTime.Now);
                //更新商户图片
                UpdateBusinessPic();
                LogHelper.LogWriter("更新商户图片完成");

                //更新骑士图片
                UpdateClienterPic();
                logger.Info("更新骑士图片完成");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }


        #region 更新商户图片

        private void UpdateBusinessPic()
        {
            BusinessDao businessDao = new BusinessDao();
            var list = businessDao.GetBusinessPics();
            foreach (var record in list)
            {
                if (!string.IsNullOrEmpty(record.CheckPicUrl))
                {
                    var sourcePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath + record.CheckPicUrl;
                    if (!File.Exists(sourcePath))
                    {
                        logger.Error(string.Format("商家图片不存在:{0}", sourcePath));
                    }
                    else
                    {
                        var destPath = BusinessBasePath + record.CheckPicUrl;
                        ProcessDirect(destPath);
                        FileHelper.Copy(sourcePath, destPath);
                        logger.Info(string.Format("copy business:{0}  {1} to {2}", record.Id, sourcePath, destPath));
                    }
                }
                if (!string.IsNullOrEmpty(record.BusinessLicensePic))
                {
                    var sourcePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath + record.BusinessLicensePic;
                    if (!File.Exists(sourcePath))
                    {
                        logger.Error(string.Format("商家图片不存在:{0}",sourcePath));
                    }
                    else
                    {
                        var destPath = BusinessBasePath + record.BusinessLicensePic;
                        ProcessDirect(destPath);
                        FileHelper.Copy(sourcePath, destPath);
                        logger.Info(string.Format("copy business:{0}  {1} to {2}", record.Id, sourcePath, destPath));
                    }
                }
            }
        }

        #endregion

        #region 更新骑士图片

        private void UpdateClienterPic()
        {
            ClienterDao clienterDao = new ClienterDao();
            var list = clienterDao.GetClienterPics();
            foreach (var record in list)
            {
                if (!string.IsNullOrEmpty(record.PicUrl))
                {
                    var sourcePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath + record.PicUrl;
                    if (!File.Exists(sourcePath))
                    {
                        logger.Error(string.Format("骑士图片不存在:{0}", sourcePath));
                    }
                    else
                    {
                        var destPath = ClienterBasePath + record.PicUrl;
                        ProcessDirect(destPath);
                        FileHelper.Copy(sourcePath, destPath);
                        logger.Info(string.Format("copy clienter:{0}  {1} to {2}", record.Id, sourcePath, destPath));
                    }
                }
                if (!string.IsNullOrEmpty(record.PicWithHandUrl))
                {
                    var sourcePath = Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath + record.PicWithHandUrl;
                    if (!File.Exists(sourcePath))
                    {
                        logger.Error(string.Format("骑士图片不存在:{0}", sourcePath));
                    }
                    else
                    {
                        var destPath = ClienterBasePath + record.PicWithHandUrl;
                        ProcessDirect(destPath);
                        FileHelper.Copy(sourcePath, destPath);
                        logger.Info(string.Format("copy clienter:{0}  {1} to {2}", record.Id, sourcePath, destPath));
                    }
                }
            }
        }

        #endregion

        private void ProcessDirect(string dir)
        {
            var destDirectoryName = Path.GetDirectoryName(dir);
            if (!Directory.Exists(destDirectoryName))
            {
                Directory.CreateDirectory(destDirectoryName);
            }
        }
    }
}
