using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderPushRecord
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 骑士Id集合
        /// </summary>
        public string ClienterIdList { get; set; }
        /// <summary>
        /// 任务类型(0:非店内任务 1:店内任务)
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime PushTime { get; set; }
        /// <summary>
        /// 推送次数
        /// </summary>
        public int PushCount { get; set; }
        /// <summary>
        /// 推送骑士数量
        /// </summary>
        public int ClienterCount { get; set; }
        /// <summary>
        /// 推送是否成功(0:失败 1:成功)
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
