using Ets.Model.Common;
using Ets.Model.DataModel.Authority;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Authority
{
    public class AuthorityManage
    {
        public AuthorityManageList authorityManageList { get; set; }
    }
    public class AuthorityManageList
    {
        public AuthorityManageList(List<account> _accountModel, NewPagingResult pagingResult)
        {
            accountModel = _accountModel;
            PagingResult = pagingResult;
        }
        public List<account> accountModel { get; set; }
        public NewPagingResult PagingResult { get; set; }
    }
}
