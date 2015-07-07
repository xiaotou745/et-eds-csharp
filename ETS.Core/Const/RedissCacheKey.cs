using System;
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

        public const string Ets_Dao_GlobalConfig_GlobalConfigGet = "Ets_Dao_GlobalConfig_GlobalConfigGet_{0}";
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
        public const string PostForgetPwd_C = "PostForgetPwd_C_";

        /// <summary>
        /// 商家端找回密码缓存KEY
        /// </summary>
        public const string CheckCodeFindPwd_B = "CheckCodeFindPwd_B_";

        /// <summary>
        /// 商家注册缓存key
        /// </summary>
        public const string PostRegisterInfo_B = "PostRegisterInfo_B_";

        /// <summary>
        /// 后台获取开放城市
        /// </summary>
        public const string Ets_Service_Provider_Common_GetOpenCityInfo = "Ets_Service_Provider_Common_GetOpenCityInfo";

        /// <summary>
        /// 后台登录验证码缓存key
        /// </summary>
        public const string CaptchaImage = "LoginCaptchaImage_{0}";

        /// <summary>
        /// 图片自增Key
        /// </summary>
        public const string ImageIdentity = "ImageIdentity";

        /// <summary>
        /// openapi新增订单 第三方店铺id缓存key
        /// </summary>
        public const string OtherBusinessIdInfo = "OtherBusiness_{0}_{1}";

        /// <summary>
        /// openapi新增订单 第三方订单是否已经存在key
        /// </summary>
        public const string OtherOrderInfo = "OtherOrder_{0}_{1}";
        /// <summary>
        /// 订单金额是否支付
        /// </summary>

        public const string CheckOrderPay = "CheckOrderPay_{0}";

        /// <summary>
        /// 商户对应骑士
        /// </summary>
        public const string BusinessClienter = "BusinessClienter_{0}";
    }
}
