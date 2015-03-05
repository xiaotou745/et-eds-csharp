using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class SubsidyModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 订单佣金
        /// </summary>
        public decimal? OrderCommission { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }
        /// <summary>
        /// 网站补贴
        /// </summary>
        public decimal? WebsiteSubsidy { get; set; }
        //开始时间
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public sbyte Status { get; set; }

        /// <summary>
        /// 集团id
        /// </summary>
        public int GroupId { get; set; }
        

    }
}
