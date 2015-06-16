using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// 商户 查询实体类 
    /// </summary>
    public class BusinessClienterRelationPM
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public int ClienterId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }
        
    }
}
