using Ets.Model.Common;
using Ets.Model.Common.AliPay;
using Ets.Model.ParameterModel.AliPay;
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
        /// 订单回调
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
        dynamic AlipayResult();

        /// <summary>
        /// 查询支付状态
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <returns></returns>
        dynamic GetOrderPayStatus(OrderPayModel model);
    }
}
