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
        IScheduler myScheduler = null;
        public ShortMessageService()
        {
            InitializeComponent();
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
            properties["quartz.plugin.xml.fileNames"] = "~/quartz_jobs.xml";
            ISchedulerFactory myFactory = new StdSchedulerFactory(properties);
            myScheduler = myFactory.GetScheduler();
        }

        protected override void OnStart(string[] args)
        {
            //Thread.Sleep(1000 * 10);
            myScheduler.Start();
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "服务开启");
        }

        protected override void OnStop()
        {
            myScheduler.Shutdown();
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "服务结束");
        }
    }
}
