using Ets.Model.ParameterModel.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Pay
{
    public interface IWxPayProvider
    {
        /// <summary>
        /// 商家充值
        /// 窦海超
        /// 2016年2月17日 11:49:04
        /// </summary>
        /// <returns></returns>
        string CreatePayBusinessRecharge(WxPayParam model);

       
    }
}
