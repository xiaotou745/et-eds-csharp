using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Bussiness
{
    /// <summary>
    /// 根据骑士id查询骑士绑定商家列表 返回结果 
    /// </summary>
    public class BCRelationGetByClienterIdDM
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
        /// 骑士ID
        /// </summary>
        public int ClienterId { get; set; }
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
        /// <summary>
        /// 是否绑定(0:否 1:是)
        /// </summary>
        public int IsBind { get; set; }
    }
}
