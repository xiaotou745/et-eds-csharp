using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using ETS.Util;

namespace Ets.AddressAssociate
{
    public class AdressAssociateBLL : Quartz.IJob
    {
        private ILog logger = LogManager.GetCurrentClassLogger();
        private static bool threadSafe = true;//线程安全
        public void Execute(Quartz.IJobExecutionContext context)
        {
            if (!threadSafe)
            {
                return;
            }
            threadSafe = false;

            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex);
            }
            finally
            {
                threadSafe = true;
            }
            //throw new NotImplementedException();
        }
    }
}
