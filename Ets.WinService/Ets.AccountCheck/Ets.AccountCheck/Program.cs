using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck
{
    class Program
    {
        static void Main(string[] args)
        {

            CheckAccountService.Check();

            //ErrorService.Add("a,a,a,a,a,a");
            //ErrorService.Add("b,b,b,b,b,b");

            //string[] lines = ErrorService.Read();

            //ErrorService.Delete();


            ////EmailService.SendEmail("lee.bmw@foxmail.com", "测试邮件", "发个文本试试");

            //ClienterAccountChecking checking = RepositoryService.GetLastStat(0);

            //IDictionary<int, ClienterAccountBalance> vlaue = RepositoryService.GetClienterAccountBalance(new int[1] { 114 });

            //IDictionary<int,ClienterFlowStat> stt = RepositoryService.FlowMoney(new int[1] { 114 }, new DateTime(2015, 5, 15), new DateTime(2015, 5, 16));

            //IList<int> list = RepositoryService.AllClienters();

            //RepositoryService.AddAccountChecking(new ClienterAccountChecking()
            //{
            //    ClienterId = 1,
            //    ClienterTotalMoney = 100.25m,
            //    FlowStatMoney = 200.23m,
            //    StartDate = DateTime.Now,
            //    CreateDate = DateTime.Now,
            //    EndDate = DateTime.Now.AddDays(100)
            //});
        }
    }
}
