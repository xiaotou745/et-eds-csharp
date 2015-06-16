using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    public partial class BusinessGroupLogModel
    {
        /// <summary>
        /// 自增ID（PK）
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 策略ID
        /// </summary>
        public int StrategyId { get; set; }      
 
        /// <summary>
        /// 写入时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
    }

}
