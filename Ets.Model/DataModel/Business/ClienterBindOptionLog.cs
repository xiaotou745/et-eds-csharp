using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    /// <summary>
    /// 商户绑定骑士操作记录表
    /// danny-20150608
    /// </summary>
    public class ClienterBindOptionLog
    {
        /// <summary>
        /// 自增Id(PK)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 骑士Id
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public int OptId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Remark { get; set; }

    }
}
