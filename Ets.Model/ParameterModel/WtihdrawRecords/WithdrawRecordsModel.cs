using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.WtihdrawRecords
{
    public class WithdrawRecordsModel
    {
        /// <summary>
        /// 平台属性：
        ///0：商家端
        ///1：配送端
        /// </summary>
        public int Platform { get; set; }

        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public int AdminId { get; set; }

    }
}
