using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;
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
    public class Job_CrossShop
    {


        public static string GetLogFilePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\logs\\log.txt";
        }
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
                    try
                    {
                        if (GlobalConfigDao.GlobalConfigGet.IsStartOverStoreSubsidies == "1")
                        {

                            List<GlobalConfigSubsidies> MyList = GetSubsidies(GlobalConfigDao.GlobalConfigGet.OverStoreSubsidies);
                            if (MyList == null || MyList.Count <= 0)
                            {
                                ETS.Util.Log.WriteTextToFile("global配置数据为空", GetLogFilePath(), true);
                                continue;
                            }
                            SubsidyProvider CrossShop = new SubsidyProvider();
                            //调用抢单奖励机制主要方法
                            if (CrossShop.CrossShop(MyList))
                            {
                                ETS.Util.Log.WriteTextToFile("跨店奖励计算成功" + DateTime.Now.ToString() + ":", GetLogFilePath(), true);
                            }
                        }
                        else
                        {
                            ETS.Util.Log.WriteTextToFile("跨店补贴未开启", GetLogFilePath(), true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ETS.Util.Log.WriteTextToFile(ex.ToString(), GetLogFilePath(), true);
                    }
                }
                Thread.Sleep(1000 * 60 * 60);
            }

        }
    }
}
