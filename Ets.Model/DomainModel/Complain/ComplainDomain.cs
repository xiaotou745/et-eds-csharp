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

        /// <summary>
        /// 是否处理
        /// </summary>
        public int IsHandle { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string HandleOpinion { get; set; }     
    }
}
