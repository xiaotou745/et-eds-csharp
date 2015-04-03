using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using System.Threading;
using System.ServiceProcess;
using ETS;

namespace Ets.TimeSubsidies
{
    public partial class TimeSubsidies : ServiceBase
    {
        public TimeSubsidies()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Thread.Sleep(1000 * 10);
            Thread t = new Thread(ExecTimeSubsidies);
            t.Start();
        }

        protected override void OnStop()
        {
            ETS.Util.LogHelper.LogWriter("服务停止了");
        }

        private static void ExecTimeSubsidies()
        {
            ETS.Util.Log.WriteTextToFile("服务开始了", Config.ConfigKey("LogPath"),true);
            while (true)
            {
                ///三十秒执行一次
                try
                {
                    Ets.Service.Provider.Order.AutoAdjustProvider autoAdjustProvider = new Ets.Service.Provider.Order.AutoAdjustProvider();
                    autoAdjustProvider.AdjustOrderService();
                }
                catch (Exception ex)
                {
                    //ETS.Util.LogHelper.LogWriter("主方法体错了:" + ex.Message);
                    ETS.Util.Log.WriteTextToFile("当前时间:" + DateTime.Now.ToString() + "主方法体错了:" + ex.Message, Config.ConfigKey("LogPath"), true);
                }
                Thread.Sleep(30 * 1000);//睡眠30秒
            }

        }
    }
}
