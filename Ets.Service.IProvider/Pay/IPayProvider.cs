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
        /// </summary>
        /// <param name="model"></param>
        ResultModel<PayResultModel> CreatePay(PayModel model);

        /// <summary>
        /// 订单回调
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel<NotifyResultModel> Notify(NotifyModel model);
    }
}
