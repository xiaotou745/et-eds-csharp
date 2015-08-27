﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ETS.Util;
using Quartz.Impl;
using System.Threading;

namespace Ets.GetAccountErr
{
    public partial class GetAccountErrService : ServiceBase
    {
        private Quartz.IScheduler scheduler;
        public GetAccountErrService()
        {
            try
            {
                InitializeComponent();
                Quartz.ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                scheduler = schedulerFactory.GetScheduler();
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("异常啦：" + ex);
            }
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