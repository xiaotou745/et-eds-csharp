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
    /// <summary>
    /// 登陆状态接口
    /// </summary>
    public enum LoginModelStatus
    {
        Success = 1,
        [DisplayText("用户名或密码错误")]
        InvalidCredential = 0
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
        BusinessEmpty= -11,
        [DisplayText("您的余额不足，请及时充值!")]
        BusiBalancePriceLack=-12


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
        FailedGetOrders = 4
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
    public enum GetClienterStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("被拒绝")]
        Refuse = 0,
        [DisplayText("未审核")]
        Audit = 2,
        [DisplayText("审核中")]
        Auditing = 3,
        [DisplayText("骑士Id错误")]
        ErrNo = -1,
        [DisplayText("获取骑士失败")]
        FailedGet = -2,
        [DisplayText("请传递版本号")]
        NoVersion = -3,
        [DisplayText("失败")]
        Failed = 101
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
        AlreadyExists,

        [DisplayText("该用户不存在")]
        NotExists
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
    public enum OrderStatus
    {
        订单待抢单 = 0,
        订单完成 = 1,
        订单已接单 = 2,
        订单已取消 = 3,
        订单已取货 = 4
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
    public enum UploadIconStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("未传过来任何变量")]
        NOFormParameter,
        [DisplayText("无效的用户")]
        InvalidUserId,
        [DisplayText("无效的订单号")]
        InvalidOrderId,
        [DisplayText("真实姓名不能为空")]
        TrueNameEmpty,
        /// <summary>
        /// 无效的文件格式
        /// </summary>
        [DisplayText("无效的文件格式")]
        InvalidFileFormat,
        /// <summary>
        /// 图片的尺寸最小为150px*150px
        /// </summary>
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
        FirstUpload,
        [DisplayText("请传递版本号")]
        NoVersion = 100,
        [DisplayText("子订单ID无效")]
        NoOrderChildId = 101
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

    public enum UserStatus
    {
        Success,
        [DisplayText("获取用户状态失败")]
        Error
    }
    /// <summary>
    /// 获取银行列表
    /// wc
    /// </summary>
    public enum BankStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("请传递版本号")]
        NoVersion = 100,
        [DisplayText("无配置文件")]
        NoXmlConfig = 101,
        [DisplayText("失败")]
        Failed = 102
    }

    public enum AliPayStatus
    {
        [DisplayText("成功")]
        success = 1,
        [DisplayText("失败")]
        fail = 0
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

    public enum MealsSettleMode
    {
        [DisplayText("线下结算")]
        Status0 = 0,
        [DisplayText("线上结算")]
        Status1 = 1   
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


    /// <summary>
    /// 骑士是否绑定了商户（0：否 1：是）  或者 商户是否绑定了骑士（0：否 1：是）
    /// </summary>
    public enum IsBindBC
    {
        /// <summary>
        /// 最新订单
        /// </summary>
        [DisplayText("是")]
        Yes = 1,
        /// <summary>
        /// 附近订单
        /// </summary>
        [DisplayText("否")]
        No = 0,
    }
    public enum OneKeyPubOrderUpdateStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("失败")]
        Failed = 0,
        [DisplayText("只有一键发单才可以修改收货人的地址和电话")]
        OnlyOneKeyPubOrder,
        [DisplayText("传入的参数不全")]
        ParamEmpty
    }
}
