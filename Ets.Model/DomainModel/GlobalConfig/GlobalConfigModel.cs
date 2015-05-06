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
        

    }
}
