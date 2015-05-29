using Ets.Model.Common;
using Ets.Model.Common.AliPay;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.AliPay;
using Ets.Model.ParameterModel.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ResultModel<BusinessRechargeResultModel> BusinessRechargeNotify();

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
        dynamic ReturnWxpay();
    }
}
