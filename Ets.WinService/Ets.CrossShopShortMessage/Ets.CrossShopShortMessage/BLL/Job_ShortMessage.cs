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

namespace Ets.CrossShopShortMessage.BLL
{
    public class Job_ShortMessage
    {

        public static string GetLogFilePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\logs\\log.txt";
        }
       
        /// <summary>
        /// 跨店奖励主程序入口
        /// 徐鹏程
        /// 20150417
        /// </summary>
        public static void ShortMessage()
        {
            while (true)
            {
                if (DateTime.Now.Hour == ETS.Config.ShortMessageTime)
                {
                    SubsidyProvider CrossShop = new SubsidyProvider();
                    ETS.Util.Log.WriteTextToFile(DateTime.Now.ToString()+"短信发送开始", GetLogFilePath(), true);
                    CrossShop.ShortMessage();
                    ETS.Util.Log.WriteTextToFile(DateTime.Now.ToString() + "短信发送完成", GetLogFilePath(), true);
                }
                Thread.Sleep(1000 * 60 * 60);
            }

        }
    }
}
