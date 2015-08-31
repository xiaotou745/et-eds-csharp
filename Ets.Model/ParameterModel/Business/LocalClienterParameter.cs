using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    public class LocalClienterParameter
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 订单推送给骑士的区域半径(单位为公里) 
        /// </summary>
        public string PushRadius { get; set; }
    }
}
