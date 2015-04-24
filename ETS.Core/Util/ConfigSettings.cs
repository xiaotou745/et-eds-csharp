using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace ETS.Util
{
    public sealed class ConfigSettings
    {
        public static readonly ConfigSettings Instance = new ConfigSettings();

        private ConfigSettings() { }

        public string FileUploadPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadPath"];
            }
        }

        public string FileUploadFolderNameTemp
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadFolderNameTemp"];
            }
        }

        public string FileUploadFolderNameCustomerIcon
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadFolderNameCustomerIcon"];
            }
        }

        public string FileUploadFolderNameAd
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadFolderNameAd"];
            }
        }

        public string FileUploadFolderNameArtwork
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadFolderNameArtwork"];
            }
        }
        public string FileUploadFolderNameArtist
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadFolderNameArtist"];
            }
        }

        public string FileUploadFolderNameArtworkType
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadFolderNameArtworkType"];
            }
        }

        public string FileUploadFolderNameGallery
        {
            get
            {
                return ConfigurationManager.AppSettings["FileUploadFolderNameGallery"];
            }
        }

        public string ServicePhoneNumber
        {
            get
            {
                return ConfigurationManager.AppSettings["ServicePhoneNumber"];
            }
        }
        /// <summary>
        /// WebApi地址
        /// </summary>
        public string PicHost
        {
            get { return ConfigurationManager.AppSettings["WebApiAddress"]; }
        }
        /// <summary>
        /// 获取Email服务器地址
        /// </summary>
        public string EmailFromAdress
        {
            get { return ConfigurationManager.AppSettings["EmailFromAddress"]; }
        }
        /// <summary>
        /// 获取Email服务器密码
        /// </summary>
        public string EmailPwd
        {
            get { return ConfigurationManager.AppSettings["EmailFromPwd"]; }
        }
        /// <summary>
        /// 接收人邮件地址
        /// </summary>
        public string EmailToAdress
        {
            get { return ConfigurationManager.AppSettings["EmailToAddress"]; }
        }
        /// <summary>
        /// 是否发送邮件
        /// </summary>
        public string IsSendMail
        {
            get { return ConfigurationManager.AppSettings["IsSendMail"]; }
        }


        /// <summary>
        /// 是否根据集团推送订单
        /// </summary>
        public bool IsGroupPush
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["IsGroupPush"]); }
        }
        /// <summary>
        /// 开放城市代码
        /// </summary>
        public string OpenCityCode
        {
            get { return ConfigurationManager.AppSettings["OpenCityCode"]; }
        }


        public string WebSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private string[] _keywords;
        private const string KEYWORD_FILENAME = "keywords.txt";
        public string[] TextFilterKeyWords
        {
            get
            {
                if (_keywords == null)
                {
                    var filename = System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, KEYWORD_FILENAME);
                    _keywords = File.ReadAllLines(filename, Encoding.UTF8);
                }
                return _keywords;
            }
        }

        /// <summary>
        /// 当前采用的订单佣金计算规则  add by caoheyang 20150330
        /// </summary>
        public int OrderCommissionType
        {
            get { return ParseHelper.ToInt(ConfigurationManager.AppSettings["OrderCommissionType"]); }
        }
        /// <summary>
        /// 极光推送 Vip
        /// </summary>
        public string VIPName
        {
            get { return ConfigurationManager.AppSettings["VIPName"]; } 
        }
        public string IsSendVIP
        {
            get { return ConfigurationManager.AppSettings["IsSendVIP"]; }
        }

        #region 第三方对接的appkey 和 app_secret
        /// <summary>
        /// 万达appkey
        /// </summary>
        public  string WanDaAppkey
        {
            get { return ConfigurationManager.AppSettings["WanDaAppkey"]; }
        }
        /// <summary>
        ///  万达app_secret
        /// </summary>
        public string WanDaAppsecret
        {
            get { return ConfigurationManager.AppSettings["WanDaAppsecret"]; }
        }

        /// <summary>
        /// 全时appkey
        /// </summary>
        public string FulltimeAppkey
        {
            get { return ConfigurationManager.AppSettings["FulltimeAppkey"]; }
        }
        /// <summary>
        ///  全时app_secret
        /// </summary>
        public string FulltimeAppsecret
        {
            get { return ConfigurationManager.AppSettings["FulltimeAppsecret"]; }
        }

        /// <summary>
        /// 美团appkey
        /// </summary>
        public string MeiTuanAppkey
        {
            get { return ConfigurationManager.AppSettings["MeiTuanAppkey"]; }
        }
        /// <summary>
        ///  美团app_secret
        /// </summary>
        public string MeiTuanAppsecret
        {
            get { return ConfigurationManager.AppSettings["MeiTuanAppsecret"]; }
        }

        /// <summary>
        ///  美团确认订单回调接口地址
        /// </summary>
        public string MeiTuanConfirmAsyncStatus
        {
            get { return ConfigurationManager.AppSettings["MeiTuanConfirmAsyncStatus"]; }
        }

             /// <summary>
        ///  美团取消订单回调接口地址
        /// </summary>
        public string MeiTuanCancelAsyncStatus
        {
            get { return ConfigurationManager.AppSettings["MeiTuanCancelAsyncStatus"]; }
        }
             /// <summary>
        ///  美团订单配送中回调接口地址
        /// </summary>
        public string MeiTuanDeliveringAsyncStatus
        {
            get { return ConfigurationManager.AppSettings["MeiTuanDeliveringAsyncStatus"]; }
        }
              /// <summary>
        ///  美团订单已送达（订单完成）回调接口地址
        /// </summary>
        public string MeiTuanArrivedAsyncStatus
        {
            get { return ConfigurationManager.AppSettings["MeiTuanArrivedAsyncStatus"]; }
        }
  
                /// <summary>
        ///  美团回调同步订单到E代送地址 
        /// </summary>
        public string MeiTuanPullOrderInfo
        {
            get { return ConfigurationManager.AppSettings["MeiTuanPullOrderInfo"]; }
        }
  
        
        #endregion
    }
}
