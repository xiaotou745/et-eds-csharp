using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Business;

namespace Ets.Model.DomainModel.Business
{
    public class BusinessExpressRelationModel : BusinessExpressRelation
    {
        /// <summary>
        /// 操作人Id
        /// </summary>
        public int OptId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }
    }
}
