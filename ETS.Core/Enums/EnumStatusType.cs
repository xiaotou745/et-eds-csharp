using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Enums
{

    public enum EnumStatusType
    {
        审核通过 = 1,
        审核取消 = 0
    }

    public enum AppType
    {
        B端 = 1,
        C端 = 0
    }
    public enum LoginModelStatus
    {
        Success,
        [DisplayText("用户名或密码错误")]
        InvalidCredential
    }
    public enum PubOrderStatus
    {
        Success,
        [DisplayText("订单发布失败")]
        InvalidPubOrder,
        [DisplayText("订单数量不符合规则")]
        OrderCountError,
        [DisplayText("订单已经存在")]
        OrderHasExist,
        [DisplayText("您已取消资格")]
        HadCancelQualification,
        [DisplayText("商户结算比例设置异常，请联系客服")]
        BusiSettlementRatioError,
        [DisplayText("抱歉，订单金额不可低于10元")]
        AmountLessThanTen,
        [DisplayText("抱歉，订单金额不可高于5000元")]
        AmountMoreThanFiveThousand 
    }
    public enum GetOrdersStatus
    {
        Success,
        [DisplayText("获取订单失败")]
        FailedGetOrders
    }
    public enum GetOrdersNoLoginStatus
    {
        Success,
        [DisplayText("获取订单失败")]
        FailedGetOrders
    }
    public enum SendCheckCodeStatus
    {
        [DisplayText("正在发送")]
        Sending,

        [DisplayText("手机号码无效")]
        InvlidPhoneNumber,

        [DisplayText("发送失败")]
        SendFailure,
        [DisplayText("该用户已注册")]
        AlreadyExists
    }

    public enum CancelOrderStatus
    {
        [DisplayText("取消成功")]
        Success = 1,
        [DisplayText("订单来源不能为空")]
        OrderFromEmpty = 201,
        [DisplayText("订单号不能为空")]
        OrderEmpty = 202,
        [DisplayText("订单不存在")]
        OrderIsNotExist = 203,
        [DisplayText("订单已被抢,无法取消")]
        FailedCancelOrder = 204,
        [DisplayText("取消失败")]
        NotCancelOrder = 205 

    }
    public enum OrderStatus
    {
        订单待抢单 = 0,
        订单完成 = 1,
        订单已接单 = 2,
        订单已取消 = 3
    }

    public enum ModifyPwdStatus
    {
        Success,
        [DisplayText("修改密码失败")]
        FailedModifyPwd,
        [DisplayText("新密码不能为空")]
        NewPwdEmpty,
        [DisplayText("用户不存在")]
        ClienterIsNotExist,
        [DisplayText("两次密码不能相同")]
        PwdIsSame,
    }
    public enum ForgetPwdStatus
    {
        Success,
        [DisplayText("修改密码失败")]
        FailedModifyPwd,
        [DisplayText("新密码不能为空")]
        NewPwdEmpty,
        [DisplayText("用户不存在")]
        ClienterIsNotExist,
        [DisplayText("验证码不能为空")]
        checkCodeIsEmpty,
        [DisplayText("验证码错误")]
        checkCodeWrong,
        [DisplayText("您要找回的密码正是当前密码")]
        PwdIsSave,
    }

    public enum RushOrderStatus
    {
        Success,
        [DisplayText("抢单失败")]
        Failed,
        [DisplayText("超人不能为空")]
        userIdEmpty,
        [DisplayText("订单不能为空")]
        OrderEmpty,
        [DisplayText("订单不存在")]
        OrderIsNotExist,
        [DisplayText("订单已被抢或者已完成")]
        OrderIsNotAllowRush,
        [DisplayText("订单已取消")]
        OrderHadCancel,
        [DisplayText("您已取消资格")]
        HadCancelQualification


    }
    public enum FinishOrderStatus
    {
        Success,
        [DisplayText("完成订单失败")]
        Failed,
        [DisplayText("超人不能为空")]
        userIdEmpty,
        [DisplayText("订单不能为空")]
        OrderEmpty,
        [DisplayText("订单不存在")]
        OrderIsNotExist,
        [DisplayText("此订单已经是完成状态")]
        OrderIsNotAllowRush,
        [DisplayText("取货码错误")]
        PickupCodeError,
        [DisplayText("订单已取消")]
        OrderHadCancel
        
    }

    public enum GetMyBalanceStatus
    {
        Success,
        [DisplayText("失败")]
        Failed,
        [DisplayText("手机号不能为空")]
        PhoneEmpty
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
    /// 集团状态枚举  add by caohehang 20150212
    /// </summary>
    public enum GroupIsValidStatus
    {
        正常 = 1,
        不可用 = 0
    }

    /// <summary>
    /// 客服电话状态
    /// </summary>
    public enum ServicePhoneStatus
    {
        [DisplayText("成功")]
        Success = 0,
        [DisplayText("失败")]
        Failed = 1
    }

    /// <summary>
    /// 省市区信息
    /// </summary>
    public enum CityStatus
    {
        [DisplayText("最新")]
        Newest = 0,
        [DisplayText("非最新")]
        UnNewest = 1
    }
    public enum BusiAddAddressStatus : int
    {
        Success = 0,
        [DisplayText("地址不能为空")]
        AddressEmpty,
        [DisplayText("手机号不能为空")]
        PhoneNumberEmpty,
        [DisplayText("商务地址不能为空")]
        businessNameEmpty,
        [DisplayText("验证码不正确")]
        IncorrectCheckCode,
        [DisplayText("昵称已被注册")]
        NickNameAlreadyRegistered,
        [DisplayText("手机号已被注册")]
        PhoneNumberRegistered,
        [DisplayText("更新信息失败")]
        UpdateFailed
    }
    public enum UploadIconStatus
    {
        [DisplayText("成功")]
        Success,
        [DisplayText("未传过来任何变量")]
        NOFormParameter,
        [DisplayText("无效的用户")]
        InvalidUserId,
        [DisplayText("无效的订单号")]
        InvalidOrderId,
        [DisplayText("真实姓名不能为空")]
        TrueNameEmpty,

        [DisplayText("无效的文件格式")]
        InvalidFileFormat,

        [DisplayText("图片的尺寸最小为150px*150px")]
        InvalidImageSize,
        [DisplayText("上传图片失败")]
        UpFailed,
        [DisplayText("订单状态已完成且小票已全部上传不能删除")]
        DeleteFailed,
        [DisplayText("未找到该订单请联系客服")]
        CannotFindOrder,
        [DisplayText("删除失败请联系客服")]
        DeleteExcepiton,
        [DisplayText("骑士编号无效")]
        ClienterIdInvalid,
        [DisplayText("小票地址无效")]
        ReceiptAddressInvalid,
        [DisplayText("请先上传")]
        FirstUpload
    }
    public enum OrderPublicshStatus : int
    {
        [DisplayText("订单发布成功")]
        Success = 1,
        [DisplayText("订单发布失败")]
        Failed = 0,
        [DisplayText("原始订单号不能为空")]
        OriginalOrderNoEmpty = 301,

        [DisplayText("原平台商户Id不能为空")]
        OriginalBusinessIdEmpty = 302,

        [DisplayText("请确认是否已付款")]
        IsPayEmpty = 303,
        [DisplayText("收货人不能为空")]
        ReceiveNameEmpty = 304,
        [DisplayText("收货人手机号不能为空")]
        ReceivePhoneEmpty = 305,

        [DisplayText("收货人所在省不能为空")]
        ReceiveProvinceEmpty = 306,

        [DisplayText("收货人所在市不能为空")]
        ReceiveCityEmpty = 307,

        [DisplayText("收货人所在区不能为空")]
        ReceiveAreaEmpty = 308,

        [DisplayText("收货人地址不能为空")]
        ReceiveAddressEmpty = 309,

        [DisplayText("订单来源不能为空")]
        OrderFromEmpty = 310,
        [DisplayText("商户不存在,请先注册商户")]
        BusinessNoExist = 311,
        [DisplayText("该订单已存在")]
        OrderHadExist = 312,
        [DisplayText("商户未审核")]
        BusinessNotAudit = 313


    }

    public enum UserStatus
    {
        Success,
        [DisplayText("获取用户状态失败")]
        Error
    }
}
