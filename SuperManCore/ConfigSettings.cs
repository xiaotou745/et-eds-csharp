using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace SuperManCore
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
            get { return  Convert.ToBoolean(ConfigurationManager.AppSettings["IsGroupPush"]); }
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
    }
}
