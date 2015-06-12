using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck
{
    class AccountCheckError
    {
        /// <summary>
        /// 本次骑士金额
        /// </summary>
        public decimal ThisClienterMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 本次统计流水金额合计
        /// </summary>
        public decimal ThisFlowMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 上次骑士账户余额
        /// </summary>
        public decimal PreviousTotalMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 骑士ID
        /// </summary>
        public int ClienterId
        {
            get;
            set;
        }
        /// <summary>
        /// 统计开始时间
        /// </summary>
        public DateTime End
        {
            get;
            set;
        }
        /// <summary>
        /// 统计结束时间
        /// </summary>
        public DateTime Start
        {
            get;
            set;
        }
        public override string ToString()
        {
            List<String> data = new List<string>();

            data.Add(this.ThisClienterMoney.ToString());
            data.Add(this.ClienterId.ToString());
            data.Add(this.End.ToString());
            data.Add(this.PreviousTotalMoney.ToString());
            data.Add(this.Start.ToString());
            data.Add(this.ThisFlowMoney.ToString());

            return string.Join(",", data.ToArray());
        }
        public void FillProperties(string line)
        {
            string[] items = line.Split(',');

            this.ThisClienterMoney = decimal.Parse(items[0]);
            this.ClienterId = int.Parse(items[1]);
            this.End = DateTime.Parse(items[2]);
            this.PreviousTotalMoney = decimal.Parse(items[3]);
            this.Start = DateTime.Parse(items[4]);
            this.ThisFlowMoney = decimal.Parse(items[5]);
        }
    }
}
