using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck
{
    static class LogHelper
    {
        static ILog log = null;
        static LogHelper()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));

            log = LogManager.GetLogger("RollingLogFileAppender2");
        }

        public static ILog Log
        {
            get
            {
                return log;
            }
        }
    }
}
