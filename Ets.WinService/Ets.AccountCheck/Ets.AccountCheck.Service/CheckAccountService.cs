using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck
{
    static class CheckAccountService
    {
        public static void Check()
        {
            ErrorService.Delete();

            IList<int> clienterids = RepositoryService.AllClienters();

            int pageCount = (int)Math.Ceiling(clienterids.Count / 100d);
            for (int i = 0; i < pageCount; i++)
            {
                IEnumerable<int> subClienterIds = clienterids.Skip(i * 100).Take(100);

                //获得得骑士余账户金额
                IDictionary<int, ClienterAccountBalance> clienters = RepositoryService.GetClienterAccountBalance(subClienterIds.ToArray());

                var startDate = DateTime.Now;
                var endDate = startDate.AddHours(-24);

                IDictionary<int, ClienterFlowStat> flows = RepositoryService.FlowMoney(subClienterIds.ToArray(), startDate, endDate);

                Check(clienters, flows, startDate, endDate);
            }
            SendEmail(ErrorService.Read());
        }

        public static void SendEmail(string[] lines)
        {
            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine(ClienterAccountChecking.Header()+"<br />");
            if (lines != null && lines.Length > 0)
            {
                foreach (var item in lines)
                {
                    stringbuilder.AppendLine(item + "<br />");
                }
            }
            string[] emails = ConfigurationManager.AppSettings["receiveEmailList"].Split(';');
            EmailService.SendEmail(emails, "对账数据分析", stringbuilder.ToString());
        }

        private static void Check(IDictionary<int, ClienterAccountBalance> clienters, IDictionary<int, ClienterFlowStat> flows,DateTime startDate,DateTime enddate)
        {
            foreach (KeyValuePair<int, ClienterAccountBalance> item in clienters)
            {
                Decimal flowTotalMoney = 0;
                if (flows.ContainsKey(item.Key))
                {
                    flowTotalMoney = flows[item.Key].Amcount;
                }
                ClienterAccountChecking lastChecking = RepositoryService.GetLastStat(item.Key);
                if (lastChecking != null)
                {
                    var checking = new ClienterAccountChecking()
                    {
                        ClienterId = item.Key,
                        CreateDate = DateTime.Now,
                        LastTotalMoney = lastChecking.ClienterTotalMoney,
                        ClienterTotalMoney = item.Value.AccountBalance,
                        FlowStatMoney = flowTotalMoney,
                        StartDate = startDate,
                        EndDate = enddate,
                    };
                    bool isEqual = item.Value.AccountBalance == lastChecking.LastTotalMoney + flowTotalMoney;
                    if (!isEqual)
                    {
                        ErrorService.Add(checking.ToString());
                    }
                    RepositoryService.AddAccountChecking(checking);
                }
                else
                {
                    var checking = new ClienterAccountChecking()
                    {
                        ClienterId = item.Key,
                        CreateDate = DateTime.Now,
                        LastTotalMoney = item.Value.AccountBalance,
                        ClienterTotalMoney = item.Value.AccountBalance,
                        FlowStatMoney = flowTotalMoney,
                        StartDate = startDate,
                        EndDate = enddate,
                    };
                    RepositoryService.AddAccountChecking(checking);
                    System.Diagnostics.Debug.WriteLine(checking.ClienterId);
                }
            }
        }
    }
}
