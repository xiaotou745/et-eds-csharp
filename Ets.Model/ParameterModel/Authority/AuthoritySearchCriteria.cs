using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Authority
{
    public class AuthoritySearchCriteria
    {
        public NewPagingResult PagingRequest { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }
    }
}
