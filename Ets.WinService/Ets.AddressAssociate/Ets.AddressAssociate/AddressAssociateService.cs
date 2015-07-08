using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz.Impl;
using System.Threading;

namespace Ets.AddressAssociate
{
    public partial class AddressAssociateService : ServiceBase
    {
        private Quartz.IScheduler scheduler = null;
        public AddressAssociateService()
        {
            //Thread.Sleep(1000 * 5);
            InitializeComponent();
            if (scheduler == null)
            {
                var schedulerFactory = new StdSchedulerFactory();
                scheduler = schedulerFactory.GetScheduler();
            }
        }
        protected override void OnStart(string[] args)
        {
            scheduler.Start();
        }

        protected override void OnStop()
        {
            scheduler.Shutdown(false);
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
