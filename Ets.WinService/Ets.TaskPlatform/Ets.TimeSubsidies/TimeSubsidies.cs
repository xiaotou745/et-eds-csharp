using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using System.Threading;
using System.ServiceProcess;
using ETS;
using Ets.Dao.GlobalConfig;

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
            Thread.Sleep(1000 * 10);
            Thread t = new Thread(ExecTimeSubsidies);
            t.Start();
        }

        protected override void OnStop()
        {
            ETS.Util.LogHelper.LogWriter("服务停止了");
        }

        private static void ExecTimeSubsidies()
        {
            ETS.Util.LogHelper.LogWriter("时间补贴服务开始了" + DateTime.Now.ToString());
            while (true)
            {
                ///三十秒执行一次
                try
                {
                    bool IsStarTimeSubsidies = Convert.ToBoolean(ETS.Util.ParseHelper.ToInt(GlobalConfigDao.GlobalConfigGet.IsStarTimeSubsidies, 0));//是否开启动态时间补贴(0不开启,1开启)
                    if (IsStarTimeSubsidies)
                    {
                        Ets.Service.Provider.Order.AutoAdjustProvider autoAdjustProvider = new Ets.Service.Provider.Order.AutoAdjustProvider();
                        autoAdjustProvider.AdjustOrderService();
                    }
                }
                catch (Exception ex)
                {

                    //ETS.Util.LogHelper.LogWriter("主方法体错了:" + ex.Message);
                    ETS.Util.LogHelper.LogWriter("当前时间:" + DateTime.Now.ToString() + "主方法体错了:" + ex.Message);
                }

                Thread.Sleep(30 * 1000);//睡眠30秒
            }

        }
    }
}
