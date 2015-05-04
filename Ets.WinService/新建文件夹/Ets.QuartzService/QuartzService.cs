using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using Common.Logging;
using Quartz.Impl;

namespace Ets.CrossShopStatisticLogService
{
    public partial class QuartzService : ServiceBase
    {
        private Quartz.IScheduler scheduler;

        public QuartzService()
        {
            InitializeComponent();
            Quartz.ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = schedulerFactory.GetScheduler();
        }

        protected override void OnStart(string[] args)
        {
            scheduler.Start();         
        }

        protected override void OnStop()
        {
            scheduler.Shutdown();          
        }

        protected override void OnPause()
        {
            scheduler.PauseAll();
        }

        protected override void OnContinue()
        {
            scheduler.ResumeAll();
        }

    }

}
