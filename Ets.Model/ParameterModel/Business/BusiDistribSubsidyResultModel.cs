using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// 商家外送费信息Result Model
    /// add by 彭宜  20150714
    /// </summary>
    public class BusiDistribSubsidyResultModel
    {
        /// <summary>
        /// 单次配送的外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }

        /// <summary>
        /// 当前任务结算金额(每个商家设定的结算比例)    add by 彭宜   20150714
        /// </summary>
        public decimal? OrderBalance { get; set; }

        /// <summary>
        /// 剩余余额(商家余额 –当前任务结算金额)    add by 彭宜   20150714
        /// </summary>
        public decimal? RemainBalance { get; set; }
    }
}
