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
                if (RepositoryService.TestWriteConnection())
                {
                    Console.WriteLine("写数据库连接正常");
                    LogHelper.Log.Info("写数据库连接正常");
                }
                else
                {
                    Console.WriteLine("写数据库连接异常");

                    LogHelper.Log.Info("写数据库连接异常");
                    return;
                }
                if (RepositoryService.TestReadConnection())
                {
                    Console.WriteLine("读数据库连接正常");
                    LogHelper.Log.Info("读数据库连接正常");
                }
                else
                {
                    Console.WriteLine("读数据库连接异常");
                    LogHelper.Log.Info("读数据库连接异常");
                    return;
                }
                CheckAccountService.Check();

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
