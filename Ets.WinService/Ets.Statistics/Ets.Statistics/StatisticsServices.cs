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

namespace Ets.Statistics
{
    public partial class StatisticsServices : ServiceBase
    {
        public StatisticsServices()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread.Sleep(1000*10);
            Thread t = new Thread(ExecStatisticsProvider);
            t.Start();
        }
        private static void ExecStatisticsProvider()
        {
            while (true)
            {
                if (DateTime.Now.Hour.ToString().Equals(Config.ConfigKey("StartTime").ToString()))
                {
                    ///凌晨一点执行
                    Ets.Service.Provider.Statistics.StatisticsProvider statisticsProvider = new Service.Provider.Statistics.StatisticsProvider();
                    statisticsProvider.ExecStatistics();
                }
                Thread.Sleep(1000 * 60 * 60);//睡眠一小时
            }

        }

        protected override void OnStop()
        {
        }
    }
}
