﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore.Common
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
        Success = 0,
        [DisplayText("用户名或密码错误")]
        InvalidCredential = 1
    }
    public enum PubOrderStatus
    {
        Success,
        [DisplayText("订单发布失败")]
        InvalidPubOrder
    }
    public enum GetOrdersStatus
    {
        Success,
        [DisplayText("获取订单失败")]
        FailedGetOrders
    }

    public enum GetBusinessStatus
    {
        [DisplayText("获取成功")]
        Success,
        [DisplayText("获取商户失败")]
        FailedGetBusiness
    }

    public enum GetRushOrderInfoStatus
    {
        [DisplayText("获取成功")]
        Success = 1,
        [DisplayText("入口参数错误")]
        ParamError = 101
    }




    public enum GetOrdersNoLoginStatus
    {
        Success = 0,
        [DisplayText("获取订单失败")]
        FailedGetOrders = 1
    }
    public enum SendCheckCodeStatus
    {
        [DisplayText("正在发送")]
        Sending,

        [DisplayText("手机号码无效")]
        InvlidPhoneNumber,

        [DisplayText("发送失败")]
        SendFailure
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
        [DisplayText("订单不存在或订单已取消")]
        OrderIsNotExist,
        [DisplayText("订单已被抢或者已完成")]
        OrderIsNotAllowRush,
        [DisplayText("未通过审核无法抢单")]
        AuditNotPass


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
        [DisplayText("未通过审核无法完成")]
        AuditNotPass
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
    /// 集团状态枚举  add by caohehang 20150212
    /// </summary>
    public enum GroupIsValidStatus
    {
        正常 = 1,
        不可用 = 0
    }
}
