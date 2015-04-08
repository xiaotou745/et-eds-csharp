using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{

    public class HomeCountTitleModel
    {
        /// <summary>
        /// 商家总数
        /// </summary>
        public int BusinessCount { get; set; }
        /// <summary>
        /// 认证骑士数量
        /// </summary>
        public int RzqsCount { get; set; }
        /// <summary>
        /// 等待认证骑士
        /// </summary>
        public int DdrzqsCount { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderPrice { get; set; }
        /// <summary>
        /// 任务量
        /// </summary>
        public int MisstionCount { get; set; }

        /// <summary>
        /// 订单量
        /// </summary>
        public int OrderCount { get; set; }


        /// <summary>
        /// 商户平均发布订单
        /// </summary>
        public double BusinessAverageOrderCount { get; set; }

        /// <summary>
        /// 任务平均订单量
        /// </summary>
        public double MissionAverageOrderCount { get; set; }

        /// <summary>
        /// 骑士平均完成订单量
        /// </summary>
        public double ClienterAverageOrderCount { get; set; }


        /// <summary>
        /// 商户结算金额（应收）
        /// </summary>
        public decimal YsPrice { get; set; }

        /// <summary>
        /// 骑士佣金总计（应付）
        /// </summary>
        public decimal YfPrice { get; set; }

        /// <summary>
        /// 盈亏总计
        /// </summary>
        public decimal YkPrice { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public string PubDate { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal WithdrawPrice { get; set; }

        /// <summary>
        /// 补贴次数
        /// </summary>
        public int DealCount { get; set; }
        /// <summary>
        /// 一次补贴订单量
        /// </summary>
        public int OneSubsidyOrderCount { get; set; }
        /// <summary>
        /// 二次补贴订单量
        /// </summary>
        public int TwoSubsidyOrderCount { get; set; }
        /// <summary>
        /// 三次补贴订单量
        /// </summary>
        public int ThreeSubsidyOrderCount { get; set; }

        /// <summary>
        /// 未完成任务
        /// </summary>
        public int UnfinishedMissionCount { get; set; }

        /// <summary>
        /// 已完成任务
        /// </summary>
        public int FinishedMissionCount { get; set; }

        /// <summary>
        /// 未完成订单
        /// </summary>
        public int UnfinishedOrderCount { get; set; }

        /// <summary>
        /// 已完成订单
        /// </summary>
        public int FinishedOrderCount { get; set; }

    }
}
