﻿using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;

namespace ETS.Util
{  
    public class SupermanApiCaching
    {
        public static readonly SupermanApiCaching Instance = new SupermanApiCaching();

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
    public class SupermanApiConfig
    {
        public static readonly SupermanApiConfig Instance = new SupermanApiConfig();

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
        private string _SmsContentCheckCodeVoice;
        public string SmsContentCheckCodeVoice
        {
            get
            {
                if (string.IsNullOrEmpty(_SmsContentCheckCodeVoice))
                {
                    _SmsContentCheckCodeVoice = ConfigurationManager.AppSettings["SmsContentCheckCodeVoice"];
                }
                return _SmsContentCheckCodeVoice;
            }
        }

        private string _SmsContentCheckCodeFindPwdVoice;
        public string SmsContentCheckCodeFindPwdVoice
        {
            get
            {
                if (string.IsNullOrEmpty(_SmsContentCheckCodeFindPwdVoice))
                {
                    _SmsContentCheckCodeFindPwdVoice = ConfigurationManager.AppSettings["SmsContentCheckCodeFindPwdVoice"];
                }
                return _SmsContentCheckCodeFindPwdVoice;
            }
        }
    }
}
