using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.AccountCheck
{
    class ClienterAccountChecking
    {
        public int Id
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
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 明细统计金额
        /// </summary>
        public decimal FlowStatMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 上次总金额
        /// </summary>
        public decimal LastTotalMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal ClienterTotalMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 开始统计时间
        /// </summary>
        public DateTime StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 结束统计时间
        /// </summary>
        public DateTime EndDate
        {
            get;
            set;
        }

        public override string ToString()
        {
            string[] lines = new string[8];
            lines[0] = Id.ToString();
            lines[1] = ClienterId.ToString();
            lines[2] = CreateDate.ToString();
            lines[3] = FlowStatMoney.ToString();
            lines[4] = LastTotalMoney.ToString();
            lines[5] = ClienterTotalMoney.ToString();
            lines[6] = StartDate.ToString();
            lines[7] = EndDate.ToString();

            return string.Join(",", lines);
        }
        public static string Header()
        {
            string[] lines = new string[8];
            lines[0] = "日志ID";
            lines[1] = "骑士ID";
            lines[2] = "创建时间";
            lines[3] = "明细统计金额";
            lines[4] = "上次总金额";
            lines[5] = "总金额";
            lines[6] = "开始时间";
            lines[7] = "结束时间";

            return string.Join(",", lines);
        }
    }
}
