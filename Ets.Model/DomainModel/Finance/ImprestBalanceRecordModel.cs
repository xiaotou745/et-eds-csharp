using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    /// <summary>
    /// 备用金支出流水 add by 彭宜20150812
    /// </summary>
    public class ImprestBalanceRecordModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 操作金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OptTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 骑士姓名
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 骑士电话
        /// </summary>
        public string ClienterPhoneNo { get; set; }
        /// <summary>
        /// 备用金接收人
        /// </summary>
        public string ImprestReceiver { get; set; }
    }
}
