using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck.Service
{
    class CheckAccoutJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogHelper.Log.Info("任务开发执行");
            try
            {
                CheckAccountService.Check();
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("检查失败" + e.Message + e.StackTrace);
            }
            LogHelper.Log.Info("任务执行完成");

        }
    }
}
