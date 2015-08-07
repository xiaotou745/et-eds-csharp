using Ets.Model.Common;
using Ets.Model.Common.AliPay;
using Ets.Model.Common.YeePay;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.AliPay;
using Ets.Model.ParameterModel.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Pay.YeePay;

namespace Ets.Service.IProvider.Pay
{
    public interface IPayProvider
    {
        /// <summary>
        /// 生成支付宝订单
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <param name="model"></param>
        ResultModel<PayResultModel> CreatePay(PayModel model);

        /// <summary>
        /// 确认订单
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        dynamic ReturnAlipay();

        /// <summary>
        /// 订单回调
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        dynamic Notify();

        /// <summary>
        /// 商家充值
        /// 窦海超
        /// 2015年5月29日 15:09:29
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel<BusinessRechargeResultModel> BusinessRecharge(BusinessRechargeModel model);

        /// <summary>
        /// 商家充值回调方法 
        /// 窦海超
        /// 2015年5月29日 15:17:07
        /// </summary>
        /// <returns></returns>
        dynamic BusinessRechargeNotify();

        /// <summary>
        /// 查询支付状态
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <returns></returns>
        dynamic GetOrderPayStatus(OrderPayModel model);

        /// <summary>
        /// 微信支付回调方法 
        /// 窦海超
        /// 2015年5月13日 15:03:45
        /// </summary>
        /// <returns></returns>
        dynamic WxNotify();

        /// <summary>
        /// 微信商家充值回调方法 
        /// 窦海超
        /// 2015年8月6日 23:06:02
        /// </summary>
        /// <returns></returns>
        void BusinessRechargeWxNotify();

        /// <summary>
        /// 易宝转账回调接口
        /// </summary>
        /// <param name="data"></param>
        bool YeePayCashTransferCallback(string data);

        /// <summary> 
        /// 注册易宝子账户 add by caoheyang 20150722
        /// </summary>
        /// <param name="para"></param>
        RegisterReturnModel RegisterYee(YeeRegisterParameter para);

        /// <summary>
        /// 易宝提现  add by caoheyang 20150722
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        TransferReturnModel CashTransferYee(YeeCashTransferParameter model);

        /// <summary> 
        /// 易宝转账 add by caoheyang 20150722
        /// </summary>
        /// <param name="para"></param>
        TransferReturnModel TransferAccountsYee(YeeTransferParameter para);

        /// <summary>
        /// 易宝自动对账
        /// danny-20150730
        /// </summary>
        void YeePayReconciliation();
    }
}
