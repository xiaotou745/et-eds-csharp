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
        /// <summary>
        /// 推荐人手机号(骑士)
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 推荐人真实姓名 骑士
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 推荐的骑士总数量 骑士
        /// </summary>
        public int ClienterCount { get; set; }
        /// <summary>
        /// 骑士完成的任务总数
        /// </summary>
        public int TaskCount { get; set; }
        /// <summary>
        /// 骑士完成的订单总素
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 推荐人手机号商家
        /// </summary>
        public string RecommendPhone { get; set; }
        /// <summary>
        /// 推荐商家数量
        /// </summary>
        public int BusCount { get; set; }
    }


    /// <summary>
    /// 推荐人统计 --用户类型商家
    /// </summary>
    public class RecommendDetailDataModel
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string BusName { get; set; }
        /// <summary>
        /// 骑士名称
        /// </summary>
        public string CliName { get; set; }
        /// <summary>
        /// 注册手机号
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// 商户地址
        /// </summary>
        public string BusAddress { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegDateTime { get; set; }
        /// <summary>
        /// 推荐的任务总数量
        /// </summary>
        public int TaskCount { get; set; }
        /// <summary>
        /// 推荐的订单总数量
        /// </summary>
        public int OrderCount { get; set; }

    }

}
