using SuperManCore.Paging;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class SubsidyManage
    {
        public SubsidyManageList orderCountManageList { get; set; }
    }
    public class SubsidyManageList
    {
        public SubsidyManageList(List<subsidy> _subsidyModel, PagingResult pagingResult)
        {
            subsidyModel = _subsidyModel;
            PagingResult = pagingResult;
        }
        public List<subsidy> subsidyModel { get; set; }
        public PagingResult PagingResult { get; set; }
    }
}
