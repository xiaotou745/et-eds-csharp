using Ets.Model.Common;
using Ets.Model.DataModel.Subsidy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Subsidy
{
    public class SubsidyManage
    {
        public SubsidyManageList subsidyManageList { get; set; }
    }
    public class SubsidyManageList
    {
        public SubsidyManageList(List<subsidy> _subsidyModel, NewPagingResult pagingResult)
        {
            subsidyModel = _subsidyModel;
            PagingResult = pagingResult;
        }
        public List<subsidy> subsidyModel { get; set; }
        public NewPagingResult PagingResult { get; set; }
    }
}
