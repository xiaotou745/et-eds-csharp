using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.GlobalConfig
{
    [Serializable]
    public class GlobalConfigModel
    {
        /// <summary>
        ///保本利润比例
        /// </summary>
        public string CommissionRatio { get; set; }

        /// <summary>
        /// 保本网站补贴
        /// </summary>
        public string SiteSubsidies { get; set; }

        /// <summary>
        /// 按时间补贴
        /// 5分加1元
        /// 8分加1元
        /// 10分加1元 
        /// </summary>
        public string TimeSubsidies { get; set; }

        /// <summary>
        /// 佣金方式计算方式
        /// </summary>
        public string CommissionFormulaMode { get; set; } 
         
        /// <summary>
        /// 满足金额补贴利润比例
        /// </summary>
        public string PriceCommissionRatio { get; set; } 
         
        /// <summary>
        /// 满足金额网站补贴
        /// </summary>
        public string PriceSiteSubsidies { get; set; } 

        /// <summary>
        /// 是否开启动态时间补贴(0不开启,1开启)
        /// </summary>
        public string IsStarTimeSubsidies { get; set; }

        /// <summary>
        /// 按金额补贴
        /// 50补贴1元
        /// 80补贴2元
        /// 80补贴3元 
        /// </summary>
        public string PriceSubsidies { get; set; }
         
        /// <summary>
        ///普通补贴佣金比例
        /// </summary>
        public string CommonCommissionRatio { get; set; }

        /// <summary>
        /// 普通网站补贴
        /// </summary>
        public string CommonSiteSubsidies { get; set; }
        
        /// <summary>
        ///时间段佣金比例
        /// </summary>
        public string TimeSpanCommissionRatio { get; set; }

           
        /// <summary>
        ///时间段之内补贴价钱(（A）	上午10：00-13:00 下午16：00-19:00补贴N员)
        /// </summary>
        public string TimeSpanInPrice { get; set; }
            
        /// <summary>
        ///时间段之外补贴价钱 （B）	其他时间段补贴2元或者更低
        /// </summary>
        public string TimeSpanOutPrice { get; set; }
          
        /// <summary>
        ///跨店抢单补贴
        /// </summary>
        public string OverStoreSubsidies { get; set; }

        /// <summary>
        /// 是否开启跨店抢单补贴(0不开启,1开启)
        /// </summary>
        public string IsStartOverStoreSubsidies { get; set; }

        /// <summary>
        /// 骑士端有未完成订单上传一次经纬度给到服务端的时间间隔(单位为秒)
        /// </summary>
        public string HasUnFinishedOrderUploadTimeInterval { get; set; }

        /// <summary>
        /// 骑士端没有未完成订单上传一次经纬度给到服务端的时间间隔(单位为秒)
        /// </summary>
        public string AllFinishedOrderUploadTimeInterval { get; set; }

        /// <summary>
        /// 订单推送给骑士的区域半径(单位为公里)
        /// </summary>
        public string PushRadius { get; set; }

        /// <summary>
        ///骑士订单列表每页显示条数
        /// </summary>
        public string ClienterOrderPageSize { get; set; }
        /// <summary>
        ///商家专属骑士接单响应时间
        /// </summary>
        public string ExclusiveOrderTime { get; set; }

        /// <summary>
        /// 策略Id
        /// </summary>
        public int StrategyId { get; set; }
        /// <summary>
        /// 分组Id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }

        /// <summary>
        /// 骑士完成任务时间限制
        /// </summary>
        public string CompleteTimeSet { get; set; }

        /// <summary>
        /// 雇主任务时间限制
        /// </summary>
        public string EmployerTaskTimeSet { get; set; }

		/// <summary>
        /// 无效订单判定时抢单点和完成点的距离(米)
        /// </summary>
        public string GrabToCompleteDistance { get; set; }
		
		/// <summary>
        /// 无效订单判定时累计完成订单数量
        /// </summary>
        public string OrderCountSetting { get; set; }
        /// <summary>
        /// 骑士提现小于等于X元支付手续费
        /// </summary>
        public string ClienterWithdrawCommissionAccordingMoney { get; set; }
        /// <summary>
        /// 提现扣除手续费数值
        /// </summary>
        public string WithdrawCommission { get; set; }
    }
}
