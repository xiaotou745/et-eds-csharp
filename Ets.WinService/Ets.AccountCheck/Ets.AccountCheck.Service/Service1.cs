﻿using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck.Service
{
    public partial class Service1 : ServiceBase
    {
        IScheduler sched;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (RepositoryService.TestConnection())
            {
                LogHelper.Log.Info("数据库连接正常");
            }
            else
            {
                LogHelper.Log.Info("数据库连接异常");

                return;
            }

            LogHelper.Log.Info("服务已启动");

            DateTimeOffset runTime = DateBuilder.TodayAt(01, 00, 00);
            ITrigger trigger = TriggerBuilder.Create()
               .WithIdentity("trigger1", "group1")
               .StartAt(runTime).WithSchedule(SimpleScheduleBuilder.RepeatHourlyForever(24))
               .Build();

            IJobDetail detail = JobBuilder.Create<CheckAccoutJob>().WithIdentity("job1", "group1").Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            sched = sf.GetScheduler();
            sched.ScheduleJob(detail, trigger);

            sched.Start();

            LogHelper.Log.Info("调度任务已启动");
        }

        protected override void OnStop()
        {
            sched.Shutdown(false);
            LogHelper.Log.Info("服务已停止");

        }
    }
}
