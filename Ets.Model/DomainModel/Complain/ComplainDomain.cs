using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Complain;

namespace Ets.Model.DomainModel.Complain
{
    public class ComplainDomain:ComplainModel
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string BussinessName { get; set; }
        /// <summary>
        /// 骑士名称
        /// </summary>
        public string ClienterName { get; set; }
        
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
    }
}
