using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Pay.YeePay;

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

        public static string Ets_Dao_GlobalConfig_GlobalConfigGet = "Ets_Dao_GlobalConfig_GlobalConfigGet_{0}_" + ETS.Config.GlobalVersion;
        /// <summary>
        /// 获取开通城市的省市区缓存key
        /// </summary>
        public const string Ets_Service_Provider_Common_GetOpenCity = "Ets_Service_Provider_Common_GetOpenCity_{0}";
        /// <summary>
        /// 获取开通城市缓存key
        /// </summary>
        public const string Ets_Service_Provider_Common_GetOpenCity_New = "Ets_Service_Provider_Common_GetOpenCity_New";
        /// <summary>
        /// 获取银行省市缓存key
        /// </summary>
        public const string Ets_Service_Provider_Common_GetPublicBankCity_New = "Ets_Service_Provider_Common_GetPublicBankCity_New";

        /// <summary>
        /// 骑士端登录
        /// </summary>
        public const string LoginCount_C = "LoginCount_C_";

        /// <summary>
        /// 骑士端注册
        /// </summary>
        public const string RegisterCount_C = "RegisterCount_C_";

        /// <summary>
        /// 骑士端修改密码请求次数
        /// </summary>
        public const string ChangePasswordCount_C = "ChangePasswordCount_C_";
        /// <summary>
        /// 骑士端修改密码验证码
        /// </summary>
        public const string ChangePasswordCheckCode_C = "ChangePasswordCheckCode_C_";

        /// <summary>
        /// 骑士端忘记密码
        /// </summary>
        public const string PostForgetPwdCount_C = "PostForgetPwdCount_C";

        /// <summary>
        /// 商家端登录
        /// </summary>
        public const string LoginCount_B = "LoginCount_B_";

        /// <summary>
        /// 商家端注册
        /// </summary>
        public const string RegisterCount_B = "RegisterCount_B_";

        /// <summary>
        /// 商家端修改密码
        /// </summary>
        public const string ChangePasswordCount_B = "ChangePasswordCount_B_";


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

        /// <summary>
        /// 骑士获取自己的物流公司
        /// </summary>
        public const string ClienterGetDeliveryCompany = "Clienter_GetDeliveryCompany_{0}";
        /// <summary>
        /// 商家提现锁
        /// </summary>
        public const string Ets_Withdraw_Lock_B = "Ets_Withdraw_Lock_B_{0}";
        /// <summary>
        /// 骑士提现锁
        /// </summary>
        public const string Ets_Withdraw_Lock_C = "Ets_Withdraw_Lock_C_{0}";
        /// <summary>
        /// 商家提现单处理状态
        /// </summary>
        public const string Ets_Withdraw_Deal_B = "Ets_Withdraw_Deal_B_{0}";
        /// <summary>
        /// 骑士提现单处理状态
        /// </summary>
        public const string Ets_Withdraw_Deal_C = "Ets_Withdraw_Deal_C_{0}";
        /// <summary>
        /// 商家提现单创建锁
        /// </summary>
        public const string Ets_Withdraw_Create_Lock_B = "Ets_Withdraw_Create_Lock_B_{0}";

    }
}
