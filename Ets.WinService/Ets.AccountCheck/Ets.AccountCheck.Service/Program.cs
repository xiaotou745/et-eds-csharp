using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck.Service
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            


            if (args.Length > 0)
            {
                string[] lines = new string[3];
                lines[0] = "1,1,1,1,1,1,1,1,1";
                lines[1] = "1,1,1,1,1,1,1,1,1";
                lines[2] = "1,1,1,1,1,1,1,1,1";

                CheckAccountService.SendEmail(lines);

                //DateTimeOffset runTime = DateBuilder.TodayAt(00, 59, 59);
                //ITrigger trigger = TriggerBuilder.Create()
                //   .WithIdentity("trigger1", "group1")
                //   .StartAt(runTime).WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(10))
                //   .Build();

                //IJobDetail detail = JobBuilder.Create<CheckAccoutJob>().WithIdentity("job1", "group1").Build();

                //ISchedulerFactory sf = new StdSchedulerFactory();
                //IScheduler sched = sf.GetScheduler();
                //sched.ScheduleJob(detail, trigger);

                //sched.Start();
                Console.ReadLine();

            }
            else
            {

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new Service1() 
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
