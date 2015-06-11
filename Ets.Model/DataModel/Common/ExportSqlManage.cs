using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Common
{

    /// <summary>
    /// sql定时导出发送数据   实体类ExportSqlManageDTO 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-06-01 14:54:42
    /// </summary>
    public class ExportSqlManage
    {
        /// <summary>
        /// 自增ID(PK)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// sql模板名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// sql语句
        /// </summary>
        public string SqlText { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public string Executetime { get; set; }
        /// <summary>
        /// 收件人邮箱，多个收件人以";"分隔
        /// </summary>
        public string ReceiveEmail { get; set; }
        /// <summary>
        /// 是否启用 0启用 1弃用 默认0
        /// </summary>
        public int IsEnable { get; set; }

    }

}
