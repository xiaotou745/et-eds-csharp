using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    /// <summary>
    /// 记录请求日志到 mq
    /// caoheyang  20160106
    /// </summary>
    public class ActionLog
    {
        public int userID { get; set; }
        public string userName { get; set; }
        public string methodName { get; set; }
        public string param { get; set; }
        public string decryptMsg { get; set; }
        public string exception { get; set; }
        public string stackTrace { get; set; }
        public long? executeTime { get; set; }
        public string sourceSys { get; set; }
        public string requestUrl { get; set; }
        public string requestTime { get; set; }
        public string requestEndTime { get; set; }
        public string appServer { get; set; }
        public int requestType { get; set; }
        public string clientIp { get; set; }
        public string requestMethod { get; set; }
        public string contentType { get; set; }
        public string header { get; set; }
        public string resultJson { get; set; }
    }
}
