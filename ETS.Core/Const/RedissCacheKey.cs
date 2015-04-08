﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Const
{
    public class RedissCacheKey
    {
        /// <summary>
        /// 商户用户状态缓存key
        /// </summary>
        public const string BusinessProvider_GetUserStatus = "BusinessProvider_GetUserStatus_{0}";
        /// <summary>
        /// 骑士用户状态缓存key
        /// </summary>
        public const string ClienterProvider_GetUserStatus = "ClienterProvider_GetUserStatus_{0}";
        /// <summary>
        /// 客服电话缓存key
        /// </summary>
        public const string Ets_Service_Provider_Common_ServicePhone = "Ets_Service_Provider_Common_ServicePhone";
        /// <summary>
        /// GetNationalAreaInfo缓存key
        /// </summary>
        public const string Common_GetNationalAreaInfo = "Ets.Service.Provider.Common_GetNationalAreaInfo";

        public const string Ets_Dao_GlobalConfig_GlobalConfigGet = "Ets_Dao_GlobalConfig_GlobalConfigGet";
        /// <summary>
        /// 获取开通城市的省市区缓存key
        /// </summary>
        public const string Ets_Service_Provider_Common_GetOpenCity = "Ets_Service_Provider_Common_GetOpenCity_{0}";
        /// <summary>
        /// 获取开通城市缓存key
        /// </summary>
        public const string Ets_Service_Provider_Common_GetOpenCity_New = "Ets_Service_Provider_Common_GetOpenCity_New";
        /// <summary>
        /// C端注册验证码缓存key
        /// </summary>
        public const string PostRegisterInfo_C = "PostRegisterInfo_C_";

        /// <summary>
        /// C端找回密码验证码缓存key
        /// </summary>
        public const string PostForgetPwd_C= "PostForgetPwd_C_";
/// <summary>
/// 后台获取开放城市
/// </summary>
        public const string Ets_Service_Provider_Common_GetOpenCityInfo = "Ets_Service_Provider_Common_GetOpenCityInfo";

        /// <summary>
        /// 后台登录验证码缓存key
        /// </summary>
        public const string CaptchaImage = "LoginCaptchaImage_{0}";
    }
}
