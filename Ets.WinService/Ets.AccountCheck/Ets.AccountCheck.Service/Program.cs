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
              var dailyCalendar = new DailyCalendar("00:01", "23:59");
            dailyCalendar.InvertTimeRange = true;
            sched.AddCalendar("cal1", dailyCalendar, false, false);

            Quartz.IScheduler sch= StdSchedulerFactory.GetDefaultScheduler();
            sch.AddCalendar("test",new )

                Quartz.CalendarIntervalScheduleBuilder.Create()


            if (args.Length > 0)
            {
                Console.Read();

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
