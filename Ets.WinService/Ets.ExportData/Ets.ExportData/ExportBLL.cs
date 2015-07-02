using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.ParameterModel.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using ETS.Util;
//using NLog;

namespace Ets.ExportData
{
    public class ExportBLL : Quartz.IJob
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
            try
            {
                IExportSqlManageProvider xx = new ExportSqlManageProvider();
                xx.QueryForWindows(new DataManageSearchCriteria());
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
