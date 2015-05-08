using Ets.CrossShopService.BLL;
using ETS;
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

namespace Ets.CrossShopService
{
    public partial class CrossShopService : ServiceBase
    {
        public CrossShopService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Thread.Sleep(1000 * 10);
            //ETS.Util.Log.WriteTextToFile(DateTime.Now.ToString() + "跨店奖励服务开启", Job_CrossShop.GetLogFilePath(), true);
            ETS.Util.LogHelper.LogWriter("跨店奖励服务开启:");
            Thread t = new Thread(Job_CrossShop.RunCrossShop);
            t.Start();  
        }

        protected override void OnStop()
        {
            //ETS.Util.Log.WriteTextToFile(DateTime.Now.ToString() + "跨店奖励服务结束", Job_CrossShop.GetLogFilePath(), true);
            ETS.Util.LogHelper.LogWriter("跨店奖励服务结束:");
        }
    }
}
