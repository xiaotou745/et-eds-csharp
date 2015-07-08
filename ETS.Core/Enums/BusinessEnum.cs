using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Enums
{
    public enum MealsSettleMode
    {
        [DisplayText("线下结算")]
        LineOff = 0,
        [DisplayText("线上结算")]
        LineOn = 1
    }
    /// <summary>
    /// 修改外卖费状态
    /// </summary>
    public enum DistribSubsidyStatus
    {
        [DisplayText("成功")]
        Success,
        [DisplayText("失败")]
        Failed
    }       
    /// <summary>
    /// 状态:0未审核，1已通过，2未审核且未添加地址，3审核中，4审核被拒绝
    /// </summary>
    public enum BusinessStatus
    {
        [DisplayText("未审核")]
        Status0 = 0,
        [DisplayText("已通过")]
        Status1 = 1,
        [DisplayText("未审核且未添加地址")]
        Status2 = 2,
        [DisplayText("审核中")]
        Status3 = 3,
        [DisplayText("审核被拒绝")]
        Status4 = 4
    }

    /// <summary>
    /// 商户注册 返回枚举
    /// </summary>
    public enum BusiRegisterStatusType
    {
        [DisplayText("成功")]
        Success = 0,
        [DisplayText("签名错误")]
        SignError = 10000,
        [DisplayText("系统错误")]
        SystemError = 10001,
        [DisplayText("参数错误")]
        ParaError = 10002
    }

    public enum BusinessRegisterStatus : int
    {
        [DisplayText("注册成功")]
        Success = 0,
        [DisplayText("商户名称不能为空")]
        BusiNameEmpty = 101,
        [DisplayText("城市不能为空")]
        cityIdEmpty = 102,
        [DisplayText("手机号不能为空")]
        PhoneNumberEmpty = 103,
        [DisplayText("密码不能为空")]
        PasswordEmpty = 104,
        [DisplayText("验证码不正确")]
        IncorrectCheckCode = 105,
        [DisplayText("昵称已被注册")]
        NickNameAlreadyRegistered = 106,
        [DisplayText("手机号已被注册")]
        PhoneNumberRegistered = 107,
        [DisplayText("您输入的的号码不存在,请检查并修改")]
        PhoneNumberNotExist = 108,
        [DisplayText("原平台商户Id不能为空")]
        OriginalBusiIdEmpty = 109,
        [DisplayText("原平台商户Id已注册")]
        OriginalBusiIdRepeat = 110,
        [DisplayText("商户地址省市区地址不能为空")]
        BusiAddressEmpty = 111,
        [DisplayText("集团Id不能为空")]
        GroupIdEmpty = 112,
        [DisplayText("请填写佣金类型")]
        CommissionTypeIdEmpty = 113,
        [DisplayText("推荐人手机号不存在")]
        RecommendPhoneNoExist= 114,
        [DisplayText("推荐人手机号有误")]
        RecommendPhoneError = 115
    }


    public enum GetBussinessStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("未审核")]
        Audit = 0,
        [DisplayText("未审核且未添加地址")]
        AuditAddress = 2,
        [DisplayText("审核中")]
        Auditing = 3,
        [DisplayText("被拒绝")]
        Refuse = 4,
        [DisplayText("商户Id错误")]
        ErrNo = -1,
        [DisplayText("获取商户失败")]
        FailedGet = -2,
        [DisplayText("请传递版本号")]
        NoVersion = -3,
        [DisplayText("失败")]
        Failed = 101
    }
    public enum DeliveryStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("失败")]
        Fail = 0 ,
        [DisplayText("已存在")]
        HadExist = 2

    }

    public enum BusiAddAddressStatus : int
    {
        Success = 0,
        /// <summary>
        /// 地址不能为空
        /// </summary>
        [DisplayText("地址不能为空")]
        AddressEmpty,
        /// <summary>
        /// 手机号不能为空
        /// </summary>
        [DisplayText("手机号不能为空")]
        PhoneNumberEmpty,
        /// <summary>
        ///  商务地址不能为空
        /// </summary>
        [DisplayText("店铺名称不能为空")]
        BusinessNameEmpty,
        /// <summary>
        /// 验证码不正确
        /// </summary>
        [DisplayText("验证码不正确")]
        IncorrectCheckCode,
        /// <summary>
        /// 昵称已被注册
        /// </summary>
        [DisplayText("昵称已被注册")]
        NickNameAlreadyRegistered,
        /// <summary>
        /// 手机号已被注册
        /// </summary>
        [DisplayText("手机号已被注册")]
        PhoneNumberRegistered,
        /// <summary>
        /// 更新信息失败
        /// </summary>
        [DisplayText("更新信息失败")]
        UpdateFailed
    }
    /// <summary>
    /// B端修改商家中心接口 返回值枚举
    /// </summary>
    public enum UpdateBusinessInfoBReturnEnums
    {
        /// <summary>
        /// 成功
        /// </summary>
        [DisplayText("成功")]
        Success = 1,
        /// <summary>
        /// 无效的文件格式
        /// </summary>
        [DisplayText("无效的文件格式")]
        InvalidFileFormat = 2,
        /// <summary>
        /// 上传图片失败
        /// </summary>
        [DisplayText("上传图片失败")]
        UpFailed = 3,
        /// <summary>
        /// 无效的用户
        /// </summary>
        [DisplayText("无效的用户")]
        InvalidUserId = 4,
        /// <summary>
        /// 手机号不能为空
        /// </summary>
        [DisplayText("手机号不能为空")]
        PhoneNumberEmpty = 5,
        /// <summary>
        /// 地址不能为空
        /// </summary>
        [DisplayText("地址不能为空")]
        AddressEmpty = 6,
        /// <summary>
        ///  商务地址不能为空
        /// </summary>
        [DisplayText("店铺名称不能为空")]
        BusinessNameEmpty = 7,
        ///<summary>
        /// 更新信息失败
        /// </summary>
        [DisplayText("更新信息失败")]
        UpdateFailed = 8
    }

}
