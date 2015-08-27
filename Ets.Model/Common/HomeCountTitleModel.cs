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
        /// 零次补贴订单量
        /// </summary>
        public int ZeroSubsidyOrderCount { get; set; }
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
        /// 未完成任务量
        /// </summary>
        public int UnfinishedMissionCount { get; set; }

        /// <summary>
        /// 未被抢任务量
        /// </summary>
        public int UnGrabMissionCount { get; set; }

        /// <summary>
        /// 活跃骑士
        /// </summary>
        public int ActiveClienter { get; set; }

        /// <summary>
        /// 活跃商家 
        /// </summary>
        public int ActiveBusiness { get; set; }


        /// <summary>
        /// 跨店总金额
        /// </summary>
        public decimal CrossShopPrice { get; set; }

        /// <summary>
        /// 扫码支付总计
        /// </summary>
        public decimal userTotal { get; set; }

        /// <summary>
        /// 骑士代付总计
        /// </summary>
        public decimal clienterTotal { get; set; }

        /// <summary>
        /// 账户收入总计
        /// </summary>
        public decimal incomeTotal { get; set; }

        /// <summary>
        /// 扫码/代付总计
        /// </summary>
        //public decimal incomeTotal { get; set; }

        /// <summary>
        /// 商户充值总计
        /// </summary>
        public decimal rechargeTotal { get; set; }
        /// <summary>
        /// 系统充值
        /// </summary>
        public decimal SystemRecharge { get; set; }
        /// <summary>
        /// 系统赠送
        /// </summary>
        public decimal SystemPresented { get; set; }
        /// <summary>
        /// 客户端充值
        /// </summary>
        public decimal ClientRecharge { get; set; }
        /// <summary>
        /// 支付宝充值
        /// </summary>
        public decimal ZhiFuBaoRecharge { get; set; }
        /// <summary>
        /// 微信充值
        /// </summary>
        public decimal WeiXinRecharge { get; set; }
        /// <summary>
        /// 账户收入总计
        /// </summary>
        public decimal allIncomeTotal { get; set; }

        /// <summary>
        /// 骑士已提现佣金-实付
        /// </summary>
        public decimal withdrawClienterPrice { get; set; }

        /// <summary>
        /// 商家余额总计-应付
        /// </summary>
        public decimal businessBalance { get; set; }

        /// <summary>
        /// 商家已提款金额-实付
        /// </summary>
        public decimal withdrawBusinessPrice { get; set; }
    

    }
}
