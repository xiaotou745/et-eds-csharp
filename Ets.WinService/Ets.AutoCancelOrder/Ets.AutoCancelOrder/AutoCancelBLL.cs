using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ets.Service.Provider.Order;
using ETS.Util;

namespace Ets.AutoCancelOrder
{
    public class AutoCancelBLL : Quartz.IJob
    {
        //使用Common.Logging.dll日志接口实现日志记录        
        private static bool threadSafe = true;//线程安全

        public void Execute(Quartz.IJobExecutionContext context)
        {

            LogHelper.LogWriter("执行啦:" + DateTime.Now);
            if (!threadSafe)
            {
                return;
            }
            threadSafe = false;
            //Thread.Sleep(10000);
            try
            {
                DateTime currTime = DateTime.Now;
                DateTime startTime = currTime.AddDays(-1);
                DateTime endTime = currTime;
                string strStartTime = startTime.ToShortDateString();
                string strEndTime = endTime.ToShortDateString();
                OrderChildProvider orderChildProvider = new OrderChildProvider();
                orderChildProvider.AutoCancelOrder(strStartTime, strEndTime);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex);
            }
            finally
            {
                threadSafe = true;
            }


        }
    }
}
