using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
namespace SuperManWebApi
{
    public class SupermanApiCaching1
    {
        public static readonly SupermanApiCaching1 Instance = new SupermanApiCaching1();

        public void Add(string key, string value)
        {
            if (HttpRuntime.Cache[key] != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
            HttpRuntime.Cache.Add(key, value, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        public string Get(string key)
        {
            var obj = HttpRuntime.Cache[key];
            if (obj == null)
            {
                return null;
            }
            return obj.ToString();
        }
    }
    public class SupermanApiConfig1
    {
        public static readonly SupermanApiConfig1 Instance = new SupermanApiConfig1();

        private string _smsContentCheckCode;
        public string SmsContentCheckCode
        {
            get
            {
                if (string.IsNullOrEmpty(_smsContentCheckCode))
                {
                    _smsContentCheckCode = ConfigurationManager.AppSettings["SmsContentCheckCode"];
                }
                return _smsContentCheckCode;
            }
        }

        private string _smsContentPassword;
        public string SmsContentFindPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_smsContentPassword))
                {
                    _smsContentPassword = ConfigurationManager.AppSettings["SmsContentCheckCodeFindPwd"];
                }
                return _smsContentPassword;
            }
        }
    }
}