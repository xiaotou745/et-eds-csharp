using Ets.Service.Provider.Clienter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.CrossShopStatisticLog
{   
    /// <summary>
    /// 实现IJob接口
    /// </summary>
    public class CrossShopStatisticLogService : Quartz.IJob
    {
        //使用Common.Logging.dll日志接口实现日志记录        
        #region IJob 成员

        public void Execute(Quartz.IJobExecutionContext context)
        {
            try
            {
                int daysAgo = ETS.Config.ClienterCrossShopLogDaysAgo;
                CrossShopStatisticLogProvider CrossShopLog = new CrossShopStatisticLogProvider();
                if (CrossShopLog.InsertDataCrossShopStatisticLog(daysAgo))
                {
                    ETS.Util.LogHelper.LogWriter("获取跨店奖励统计计算成功" + DateTime.Now.ToString() + ":");
                }
                else
                {
                    ETS.Util.LogHelper.LogWriter("获取跨店奖励统计计算失败" + DateTime.Now.ToString() + ":");
                }        
     

            }           
            catch (Exception ex)
            {
                ETS.Util.LogHelper.LogWriter("5:"+ex.Message+ DateTime.Now.ToString());
            }

        }

        #endregion
    }
}
