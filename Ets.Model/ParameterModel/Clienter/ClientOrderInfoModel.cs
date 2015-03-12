using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Clienter
{
    public class ClientOrderInfoModel
    {
        /// <summary>
        /// userId
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double latitude { get; set; }
        /// <summary>
        /// 是否最新任务
        /// </summary>
        public bool isLatest { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int? pageSize { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int? pageIndex { get; set; }
        public sbyte? status { get; set; }
        //城市名称
        public string city { get; set; }
        //城市Id
        public string cityId { get; set; }
        ///// <summary>
        ///// 是否送餐订单：1送餐订单，2收锅订单
        ///// </summary>
        //public int OrderType { get; set; }

    }
}
