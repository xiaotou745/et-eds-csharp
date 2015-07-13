using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Statistics
{
    /// <summary>
    /// 返回的推荐人数据
    /// </summary>
    public class RecommendDataModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商家电话
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 商家地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 推荐人手机号
        /// </summary>
        public string RecommendPhone { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 商家审核状态
        /// </summary>
        public byte Status { get; set; }
    }


    //public class ResultRecommendDataList
    //{
    //    public IList<RecommendDataModel> DataList { get; set; }

    //    public int TotalCount { get}
    //}
}
