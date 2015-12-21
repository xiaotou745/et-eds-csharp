using Ets.Model.Common;
using Ets.Model.Common.AliPay;
using Ets.Model.Common.YeePay;
using Ets.Model.DomainModel.Business;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.AliPay;
using Ets.Model.ParameterModel.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.ParameterModel.Finance;
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
        /// 现金支付 wc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel<PayResultModel> CashPay(PayModel model);
         
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
        /// 商家充值回调方法 
        /// 胡灵波
        /// 2015年12月21日 15:42:49
        /// </summary>
        /// <returns></returns>
        dynamic SSBusinessRechargeNotify();
        /// <summary>
        /// 查询支付状态
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <returns></returns>
        dynamic GetOrderPayStatus(OrderPayModel model);
      

        /// <summary>
        /// 微信商家充值回调方法 
        /// 窦海超
        /// 2015年8月6日 23:06:02
        /// </summary>
        /// <returns></returns>
        void SSBusinessRechargeWxNotify();

        /// <summary>
        /// 微信商家充值回调方法 
        /// 窦海超
        /// 2015年8月6日 23:06:02
        /// </summary>
        /// <returns></returns>
        void BusinessRechargeWxNotify();

        QueryCashStatusReturnModel QueryCashStatusYee(YeeQueryCashStatusParameter model);
        /// <summary>
        /// 易宝转账骑士回调接口
        /// 窦海超 
        /// 2015年8月26日 20:07:16
        /// </summary>
        bool YeePayCashTransferClienterCallBack();


        /// <summary>
        /// 易宝转账商家回调接口
        /// 窦海超 
        /// 2015年8月26日 20:07:20
        /// </summary>
        bool YeePayCashTransferBusinessCallBack();

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

        /// <summary>
        /// 根据提现Id获取易宝RequestId
        /// wc
        /// </summary>
        /// <param name="withdrawId"></param>
        /// <returns></returns>
        string GetRequestId(long withdrawId);
        /// <summary>
        /// 支付宝批量付款到账
        /// 茹化肖
        /// 2015年10月20日09:19:06
        /// </summary>
        /// <param name="type">1根据提现单ID进行打款.2 将已有批次号再次提交</param>
        /// <param name="data">type=1:以英文逗号分隔的提现单ID序列 type=2:已存在的批次号</param>
        /// <returns></returns>
        string AlipayBatchTransfer(AlipayBatchPM par);
        /// <summary>
        /// 支付宝转账回调
        /// 茹化肖
        /// 2015年10月20日14:20:24
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AlipayTransferCallback(AlipayBatchCallBackModel model);

        /// <summary>
        /// 微信支付回调方法 
        /// 窦海超
        /// 2015年5月13日 15:03:45
        /// </summary>
        /// <returns></returns>
        void WxNotify();

        #region 闪送模式
        /// <summary>
        /// 生成订单 闪送
        /// 胡灵波
        /// 2015年12月8日 11:14:04
        /// </summary>
        ResultModel<PayResultModel> CreateFlashPay(PayModel model);

        /// <summary>
        /// 闪送模式 支付宝回调
        /// 胡灵波
        /// 2015年12月8日 11:14:04
        dynamic NotifyTip();

        /// <summary>
        /// 闪送模式 微信支付回调方法 
        /// 窦海超
        /// 2015年5月13日 15:03:45
        /// </summary>
        /// <returns></returns>
        void SSWxNotify();

        #endregion
    }
}
