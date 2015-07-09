using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Enums
{
    public enum OrderStatus
    { 
        [DisplayText("待接单")]
        Status0 = 0,     
        [DisplayText("订单已完成")]
        Status1 = 1,     
        [DisplayText("订单已接单")]
        Status2 = 2,    
        [DisplayText("订单已取消")]
        Status3 = 3,     
        [DisplayText("订单已取货")]
        Status4 = 4,   
        [DisplayText("第三方待接入订单")]
        Status30 = 30
    }

    /// <summary>
    /// 订单结算类型
    /// </summary>
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150515</UpdateTime>
    public enum OrderCommissionType
    {
        /// <summary>
        /// 固定比例
        /// </summary>
        [DisplayText("固定比例")]
        FixedRatio = 1,
        /// <summary>
        /// 固定金额
        /// </summary>
        [DisplayText("固定金额")]
        FixedAmount = 2
    }

    /// <summary>
    /// api 订单接口返回枚举
    /// add by caoheyang 20150317
    /// </summary>
    public enum OrderApiStatusType
    {
        /// <summary>
        /// 成功
        /// </summary>
        [DisplayText("成功")]
        Success = 0,
        /// <summary>
        /// 签名错误
        /// </summary>
        [DisplayText("签名错误")]
        SignError = 10000,
        /// <summary>
        /// 系统错误
        /// </summary>
        [DisplayText("系统错误")]
        SystemError = 10001,
        /// <summary>
        /// 参数错误
        /// </summary>
        [DisplayText("参数错误")]
        ParaError = 10002,
        /// <summary>
        /// 该订单已同步过
        /// </summary>
        [DisplayText("该订单已同步过")]
        OrderExists = 10003,
        /// <summary>
        /// 订单不存在
        /// </summary>
        [DisplayText("订单不存在")]
        OrderNotExist = 10004,
        /// <summary>
        /// 订单已经接入到E代送系统，无法取消订单
        /// </summary>
        [DisplayText("订单已经接入到e代送系统，无法取消订单")]
        OrderIsJoin = 20000,
        /// <summary>
        /// 第三方接口调用异常
        /// </summary>
        [DisplayText("第三方接口调用异常")]
        OtherError = 90000
    }

    public enum GetOrdersStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("失败")]
        Failed = 0,
        [DisplayText("请传递版本号")]
        NoVersion = 2,
        [DisplayText("订单号错误")]
        ErrOderNo = 3,
        [DisplayText("获取订单失败")]
        FailedGetOrders = 4,
        [DisplayText("账号状态出错")]
        ErrStatus = -500,
    }

    public enum OrdersStatus
    {
        [DisplayText("待接单")]
        Status0 = 0,
        [DisplayText("已完成")]
        Status1 = 1,
        [DisplayText("取货中")]
        Status2 = 2,
        [DisplayText("已取消")]
        Status3 = 3,
        [DisplayText("送货中")]
        Status4 = 4,
        [DisplayText("订单Id错误")]
        ErrId = -1,
        [DisplayText("获取订单失败")]
        FailedGet = -2,
        [DisplayText("请传递版本号")]
        NoVersion = -3,
        [DisplayText("成功")]
        Success = 100,
        [DisplayText("失败")]
        Failed = 101
    }

    /// <summary>
    /// 确认订单返回状态
    /// </summary>
    public enum ConfirmTakeStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("失败")]
        Failed = 0,
        [DisplayText("订单Id错误")]
        ErrId = -1,
        [DisplayText("获取订单失败")]
        FailedGet = -2,
        [DisplayText("请传递版本号")]
        NoVersion = -3,
        [DisplayText("请传递骑士Id")]
        ClienterIdEmpty = -4

    }

    public enum PubOrderStatus
    {
        Success = 1,
        [DisplayText("订单发布失败")]
        InvalidPubOrder = 0,
        [DisplayText("订单数量不符合规则")]
        OrderCountError,
        [DisplayText("订单已经存在")]
        OrderHasExist,
        [DisplayText("您已取消资格")]
        HadCancelQualification,
        [DisplayText("商户结算比例设置异常，请联系客服")]
        BusiSettlementRatioError,
        [DisplayText("抱歉，子订单金额不可低于5元")]
        AmountLessThanTen,
        [DisplayText("抱歉，子订单金额不可高于1000元")]
        AmountMoreThanFiveThousand,
        [DisplayText("订单金额与子订单总金额不一致")]
        AmountIsNotEqual,
        [DisplayText("订单数量与子订单数量不一致")]
        CountIsNotEqual,
        [DisplayText("合法性验证成功")]
        VerificationSuccess,
        [DisplayText("收货人手机号错误")]
        RecevicePhoneErr,
        [DisplayText("收货人手机号不能为空")]
        RecevicePhoneIsNULL,
        [DisplayText("收货人地址不能为空")]
        ReceviceAddressIsNULL,
        [DisplayText("请传递版本号")]
        NoVersion = -10,
        [DisplayText("获取商户信息失败")]
        BusinessEmpty = -11,
        [DisplayText("您的余额不足，请及时充值!")]
        BusiBalancePriceLack = -12

    }


    public enum GetOrdersNoLoginStatus
    {
        Success,
        [DisplayText("获取订单失败")]
        FailedGetOrders
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
        NotCancelOrder = 205,
        [DisplayText("取消订单失败,订单已被抢或订单不存在")]
        CancelOrderError = 206,
        [DisplayText("版本号不能为空")]
        VersionError = -3
    }
    /// <summary>
    /// 订单查询类型 1：已完成订单 2：进行中订单（已接单和已取货）
    /// danny-20150520
    /// </summary>
    public enum OrderQueryType
    {
        [DisplayText("已完成订单")]
        Success = 1,
        [DisplayText("进行中订单")]
        Working = 2

    }

    public enum RushOrderStatus
    {
        [DisplayText("抢单成功")]
        Success = 1,
        [DisplayText("抢单失败")]
        Failed = 0,
        [DisplayText("超人不能为空")]
        userIdEmpty = 100,
        [DisplayText("订单不能为空")]
        OrderEmpty = 101,
        [DisplayText("订单不存在")]
        OrderIsNotExist = 102,
        [DisplayText("订单已被抢或者已完成")]
        OrderIsNotAllowRush = 103,
        [DisplayText("订单已取消")]
        OrderHadCancel = 104,
        [DisplayText("您已取消资格")]
        HadCancelQualification = 105,
        [DisplayText("商户ID不能为空")]
        BussinessEmpty = 106,
        [DisplayText("请传递版本号")]
        NoVersion = 107,
        [DisplayText("订单号不能为空")]
        OrderNoEmpty = 108
    }
    public enum FinishOrderStatus
    {
        Success = 1,
        [DisplayText("完成订单失败")]
        Failed = 0,
        [DisplayText("超人不能为空")]
        UserIdEmpty = 101,
        [DisplayText("订单不能为空")]
        OrderEmpty = 102,
        [DisplayText("订单不存在")]
        OrderIsNotExist = 103,
        [DisplayText("此订单已经是完成状态")]
        OrderIsNotAllowRush = 104,
        [DisplayText("取货码错误")]
        PickupCodeError = 105,
        [DisplayText("订单已取消")]
        OrderHadCancel = 106,
        [DisplayText("请传递版本号")]
        NoVersion = 107,
        [DisplayText("存在未付款的子订单")]
        ExistNotPayChildOrder = 108,
        [DisplayText("数据错误")]
        DataError = 109,
        [DisplayText("订单Id为空")]
        OrderIdEmpty = 110,
        [DisplayText("亲，您完成的太快了！")]
        TooQuickly = 501

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
        BusinessNotAudit = 313,
        [DisplayText("验证成功")]
        VerificationSuccess
    }
    public enum OrderDetails
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("失败")]
        Failed = 0,
        [DisplayText("请传递版本号")]
        NoVersion
    }

    public enum OneKeyPubOrderUpdateStatus
    {
        [DisplayText("修改成功")]
        Success = 0,
        [DisplayText("修改失败")]
        Failed = 1,
        [DisplayText("只有一键发单才可以修改收货人的地址和电话")]
        OnlyOneKeyPubOrder = 2,
        [DisplayText("传入的参数不全")]
        ParamEmpty = 3
    }

    /// <summary>
    /// C端获取任务列表 类型 枚举
    /// </summary>
    public enum GetJobCMode
    {
        /// <summary>
        /// 最新订单
        /// </summary>
        [DisplayText("最新订单")]
        NewJob = 0,
        /// <summary>
        /// 附近订单
        /// </summary>
        [DisplayText("附近订单")]
        NearbyJob = 1,
        /// <summary>
        /// 雇主任务
        /// </summary>
        [DisplayText("雇主任务")]
        EmployerJob = 2
    }

}
