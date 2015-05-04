using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ets.ClienterCrossShopLog
{    
    public partial class ClienterCrossShopLogService : ServiceBase
    {
        public ClienterCrossShopLogService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "跨店奖励服务开启");
            Thread t = new Thread(Ets.CrossShopService.BLL.ClienterCrossShopLog.GetClienterCrossShopLog);
            t.Start();              
        }

        protected override void OnStop()
        {
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "跨店奖励服务结束");
        }
    }
}
