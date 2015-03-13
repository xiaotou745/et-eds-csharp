using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Util;


namespace Ets.Model.DataModel.Clienter
{
 
    public class ClientOrderSearchCriteria
    {
        public Ets.Model.Common.PagingResult PagingRequest { get; set; }
        public int userId { get; set; }
        public sbyte? status { get; set; }
        public bool isLatest { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        //城市名称
        public string city { get; set; }
        //城市Id
        public string cityId { get; set; }
        /// <summary>
        /// 订单类型：1送餐订单，2收锅订单
        /// </summary>
        public int OrderType { get; set; }

    }    
}
