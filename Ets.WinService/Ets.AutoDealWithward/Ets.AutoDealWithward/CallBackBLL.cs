using Ets.Service.Provider.Pay;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ets.AutoDealWithward
{
    public class CallBackBLL : Quartz.IJob
    {
        //使用Common.Logging.dll日志接口实现日志记录        
        private static bool threadCallBackSafe = true;//线程安全

        public void Execute(Quartz.IJobExecutionContext context)
        {

            LogHelper.LogWriter("执行啦:" + DateTime.Now);
            if (!threadCallBackSafe)
            {
                return;
            }
            threadCallBackSafe = false;
            Thread.Sleep(10000);
            try
            {
                var iPayProvider = new PayProvider();
                iPayProvider.YeePayCashTransferClienterCallBack();//骑士提现单状态变更
                iPayProvider.YeePayCashTransferBusinessCallBack();//商家提现单状态变更
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex);
            }
            finally
            {

                threadCallBackSafe = true;
            }


        }
    }
}
