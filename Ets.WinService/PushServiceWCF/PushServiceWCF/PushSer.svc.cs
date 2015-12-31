using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.AspNet.SignalR;
using System.ServiceModel.Activation;
using NLog;

namespace PushServiceWCF
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“PushSer”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 PushSer.svc 或 PushSer.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PushSer : IPushSer
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public void DoWork()
        {
        }


        /// <summary>
        /// Mobile即时推送
        /// </summary>
        /// <param name="msginfo"></param>
        public void PushForMobile(string msginfo)
        {
            try
            {
                IHubContext chat = GlobalHost.ConnectionManager.GetHubContext<PushHubMoblie>();
                chat.Clients.All.notice(msginfo);
            }
            catch (Exception e)
            {
                _log.Error(e.ToString());
            }
        }

    }
}
