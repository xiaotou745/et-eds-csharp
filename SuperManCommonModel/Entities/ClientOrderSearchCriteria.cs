using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Entities
{
    public class ClientOrderSearchCriteria
    {
        public PagingResult PagingRequest { get; set; }
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
        /// 是否送餐订单：1送餐订单，2收锅订单
        /// </summary>
        public int IsRecoveryOrder { get; set; }

    }
}
