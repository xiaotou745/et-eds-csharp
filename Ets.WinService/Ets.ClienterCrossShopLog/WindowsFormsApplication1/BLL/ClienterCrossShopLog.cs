using Ets.Dao.Clienter;
using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Subsidy;
using ETS;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ets.CrossShopService.BLL
{
    public class ClienterCrossShopLog
    {   
        /// <summary>
        /// 获取跨店奖励统计
        /// 胡灵波
        /// 20150427
        /// </summary>
        public static void  GetClienterCrossShopLog()
        {          
            while (true)
            {                           
                try
                {
                    if (DateTime.Now.Hour == ETS.Config.StartClienterCrossShopLogTime)
                    {
                        int daysAgo= ETS.Config.ClienterCrossShopLogDaysAgo;
                        ClienterCrossShopLogProvider CrossShopLog = new ClienterCrossShopLogProvider();
                        if (CrossShopLog.InsertDataClienterCrossShopLog(daysAgo))
                        {
                            ETS.Util.LogHelper.LogWriter("获取跨店奖励统计计算成功" + DateTime.Now.ToString() + ":");
                        }
                        else
                        {
                            ETS.Util.LogHelper.LogWriter("获取跨店奖励统计计算失败" + DateTime.Now.ToString() + ":");
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                        ETS.Util.LogHelper.LogWriter(ex.ToString());
                }
                Thread.Sleep(1000 * 60 * 60);           
              
            }
        }
    }
}
