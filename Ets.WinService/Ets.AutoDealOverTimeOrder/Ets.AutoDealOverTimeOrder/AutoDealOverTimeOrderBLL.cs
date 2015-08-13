using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ETS;
using Ets.Service.IProvider.Pay;
using Ets.Service.Provider.Pay;
using ETS.Util;

namespace Ets.AutoDealOverTimeOrder
{
    public class AutoDealOverTimeOrderBLL : Quartz.IJob
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
            Thread.Sleep(10000);
            try
            {
                //var iPayProvider = new PayProvider();
                //iPayProvider.AutoDealWithdrawForm();
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
