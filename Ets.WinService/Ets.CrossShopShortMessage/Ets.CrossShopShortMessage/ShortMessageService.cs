using Ets.CrossShopShortMessage.BLL;
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
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace Ets.CrossShopShortMessage
{
    public partial class ShortMessageService : ServiceBase
    {
        public ShortMessageService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Thread.Sleep(1000 * 10);
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "跨店奖励短信发送服务开启");
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
            properties["quartz.plugin.xml.fileNames"] = "~/quartz_jobs.xml";

            ISchedulerFactory myFactory = new StdSchedulerFactory(properties);
            IScheduler myScheduler = myFactory.GetScheduler();
            myScheduler.Start();
        }

        protected override void OnStop()
        {
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "跨店奖励短信发送服务结束");
        }
    }
}
