using System.Configuration;
using Common.Logging;

namespace ETS.Util
{
    public class Log
    {
        private string LogName { get; set; }
        private static Log _log;
        private static readonly object lockObject = new object();
        protected Log()
        {
            LogName = "DZLogger";
            this.Logger = LogManager.GetLogger(LogName);
        }
        protected Log(string logName)
        {
            LogName = logName;
            this.Logger = LogManager.GetLogger(LogName);
        }
        public ILog Logger { get; set; }
        public static ILog GetLogger()
        {
            lock (lockObject)
            {
                if (_log == null)
                {
                    if (_log == null)
                    {
                        var sitename = ConfigurationManager.AppSettings["SiteName"];
                        if (!string.IsNullOrEmpty(sitename))
                        {
                            _log = new Log(sitename);
                        }
                        else
                        {
                            _log = new Log();
                        }
                    }
                }
            }
            return _log.Logger;
        }
        public static ILog GetLogger(string LogName)
        {
            lock (lockObject)
            {
                if (_log == null)
                {
                    _log = new Log(LogName);
                }
            }
            return _log.Logger;
        }
    }
}
