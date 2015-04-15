using Ets.CrossShopService.BLL;
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

namespace Ets.CrossShopService
{
    public partial class CrossShopService : ServiceBase
    {
        public CrossShopService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread.Sleep(1000 * 10);
            Thread t = new Thread(Job_CrossShop.RunCrossShop);
            t.Start();  
        }

        protected override void OnStop()
        {
        }
    }
}
