using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Business;

namespace Ets.Model.DomainModel.Business
{
    public class BusinessClienterRelationModel : BusinessClienterRelation
    {
        /// <summary>
        /// 骑士姓名
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 骑士电话
        /// </summary>
        public string PhoneNo { get; set; }
    }
}
