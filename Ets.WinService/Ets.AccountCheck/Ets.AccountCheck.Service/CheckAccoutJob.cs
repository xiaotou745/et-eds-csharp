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
                if (RepositoryService.TestWriteConnection())
                {
                  
                    LogHelper.Log.Info("写数据库连接正常");
                }
                else
                {
                    LogHelper.Log.Info("写数据库连接异常");
                    return;
                }
                if (RepositoryService.TestReadConnection())
                {
                    LogHelper.Log.Info("读数据库连接正常");
                }
                else
                {
                    LogHelper.Log.Info("读数据库连接异常");
                    return;
                }
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
