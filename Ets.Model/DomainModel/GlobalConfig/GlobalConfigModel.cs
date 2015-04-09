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
        ///利润比例
        /// </summary>
        public double CommissionRatio { get; set; }

        /// <summary>
        /// 网站补贴,暂时先不用，先用以前subsidy表里的数据
        /// </summary>
        public double SiteSubsidies { get; set; }

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
        public int CommissionFormulaMode { get; set; }
         
        /// <summary>
        /// 满足金额补贴
        /// </summary>
        public string PriceSubsidies { get; set; }
         
        /// <summary>
        /// 满足金额补贴利润比例
        /// </summary>
        public double PriceCommissionRatio { get; set; } 
         
        /// <summary>
        /// 满足金额网站补贴
        /// </summary>
        public double PriceSiteSubsidies { get; set; } 

        /// <summary>
        /// 是否开启动态时间补贴(0不开启,1开启)
        /// </summary>
        public bool IsStarTimeSubsidies { get; set; }
    }
}
