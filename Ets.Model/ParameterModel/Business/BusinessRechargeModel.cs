using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    public class BusinessRechargeModel
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        public int Businessid { get; set; }

        /// <summary>
        /// 支付方式：1：支付宝；2微信
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 支付金额，必须大于0.01元
        /// </summary>
        public decimal payAmount { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }


    }
}
