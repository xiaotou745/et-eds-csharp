using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.DeliveryCompany
{
    public class DeliveryCompanyCriteria:ListParaBase
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string DeliveryCompanyName { get; set; }

        public int IsEnable { get; set; }
    }
}
