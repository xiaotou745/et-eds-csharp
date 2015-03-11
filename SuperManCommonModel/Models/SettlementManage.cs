using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class SettlementManage
    {
        public SettlementManageList SettlementMngList { get; set; }
    }

    public class SettlementManageList
    {
        public SettlementManageList(List<SettlementFucntionViewModel> _setMngViewModel, PagingResult pagingResult)
        {
            SetMngViewModelList = _setMngViewModel;
            PagingResult = pagingResult;
        }
        public List<SettlementFucntionViewModel> SetMngViewModelList { get; set; }
        public PagingResult PagingResult { get; set; }
    }
}
