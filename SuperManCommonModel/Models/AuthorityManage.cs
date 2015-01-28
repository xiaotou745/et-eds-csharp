using SuperManCore.Paging;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class AuthorityManage
    {
        public AuthorityManageList authorityManageList { get; set; }
    }
    public class AuthorityManageList
    {
        public AuthorityManageList(List<account> _accountModel, PagingResult pagingResult)
        {
            accountModel = _accountModel;
            PagingResult = pagingResult;
        }
        public List<account> accountModel { get; set; }
        public PagingResult PagingResult { get; set; }
    }
}
