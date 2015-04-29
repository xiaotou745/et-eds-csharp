using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Subsidy;
using ETS;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace Ets.CrossShopShortMessage.Jobs
{
    public class Job_ShortMessage : IJob
    {

        /// <summary>
        /// 跨店奖励主程序入口
        /// 徐鹏程
        /// 20150421
        /// </summary>
        public void Execute(IJobExecutionContext context)
        {
            SubsidyProvider CrossShop = new SubsidyProvider();
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "短信发送开始");
            CrossShop.ShortMessage();
            ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "短信发送完成");
        }
    }
}
