﻿using Ets.CrossShopShortMessage.BLL;
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
namespace Ets.CrossShopShortMessage
{
    public partial class ShortMessageService : ServiceBase
    {
        public ShortMessageService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread.Sleep(1000 * 10);
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "跨店奖励短信发送服务开启");
            Thread t = new Thread(Job_ShortMessage.ShortMessage);
            t.Start();
        }

        protected override void OnStop()
        {
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "跨店奖励短信发送服务结束");
        }
    }
}
