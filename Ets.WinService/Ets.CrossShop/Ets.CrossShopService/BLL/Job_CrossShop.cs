using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Subsidy;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ets.CrossShopService.BLL
{
    public class Job_CrossShop
    {
        /// <summary>
        /// 转换奖励机制字符串
        /// 徐鹏程
        /// 20150414
        /// </summary>
        /// <param name="OverStoreSubsidies">奖励机制规则字符串数量,金额,编号;数量,金额,编号;数量,金额,编号;</param>
        /// <returns></returns>
        private static List<GlobalConfigSubsidies> GetSubsidies(string OverStoreSubsidies)
        {
            try
            {
                if (!string.IsNullOrEmpty(OverStoreSubsidies))
                {
                    var list = new List<GlobalConfigSubsidies>();
                    string[] times = OverStoreSubsidies.Split(';');
                    if (times.Length > 0)
                    {
                        list.AddRange(from time in times
                                      where !string.IsNullOrEmpty(time)
                                      select time.Split(',')
                                          into tt
                                          select new GlobalConfigSubsidies
                                          {
                                              Id = ParseHelper.ToInt(tt[2]),
                                              Value1 = tt[0],
                                              Value2 = tt[1]
                                          });
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }

            return null;
        }
        /// <summary>
        /// 跨店奖励主程序入口
        /// 徐鹏程
        /// 20150414
        /// </summary>
        public static void RunCrossShop()
        {
            while (true)
            {
                if (DateTime.Now.Hour == ETS.Config.StartSubsidyTime)
                {
                    if (GlobalConfigDao.GlobalConfigGet.IsStartOverStoreSubsidies == "1")
                    {
                        try
                        {
                            List<GlobalConfigSubsidies> MyList = GetSubsidies(GlobalConfigDao.GlobalConfigGet.OverStoreSubsidies);
                            if (MyList == null || MyList.Count <= 0)
                            {
                                ETS.Util.LogHelper.LogWriter("global配置数据为空");
                                continue;
                            }
                            SubsidyProvider CrossShop = new SubsidyProvider();
                            //调用抢单奖励机制主要方法
                            if (CrossShop.CrossShop(MyList))
                            {
                                ETS.Util.LogHelper.LogWriter("抢单奖励计算成功");
                            }
                        }
                        catch (Exception ex)
                        {
                            ETS.Util.LogHelper.LogWriter(ex.ToString());
                        }
                    }
                    else
                    {
                        ETS.Util.LogHelper.LogWriter("跨店补贴未开启");
                    }
                }
                Thread.Sleep(1000 * 60 * 60);
            }

        }
    }
}
