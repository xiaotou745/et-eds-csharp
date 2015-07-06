using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    public class BusinessExpressRelation
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 配送公司ID
        /// </summary>
        public int ExpressId { get; set; }
        /// <summary>
        /// 是否有效(0:否 1:是)
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后一次更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 最后一次更改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
