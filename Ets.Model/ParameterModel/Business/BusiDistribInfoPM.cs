using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    public class BusiDistribInfoPM
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 外送费价格
        /// </summary>
        public decimal price { get; set; }
    }
}
