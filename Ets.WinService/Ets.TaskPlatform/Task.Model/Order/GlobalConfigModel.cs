using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task.Model.Order
{
    public class GlobalConfigModel : QueryResult
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
