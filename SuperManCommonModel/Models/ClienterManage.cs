using SuperManCore;
using SuperManCore.Paging;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class ClienterManage
    {
        public ClienterManageList clienterManageList { get; set; }
    }
    public class ClienterCountManage
    {
        public ClienterCountManageList clienterCountManageList { get; set; }
    }
    /// <summary>
    /// 统计用
    /// </summary>
    public class ClienterCountManageList
    {
        public ClienterCountManageList(List<ClienterViewModel> _clienterViewModel, PagingResult pagingResult)
        {
            clienterViewModel = _clienterViewModel;
            PagingResult = pagingResult;
        }
        public List<ClienterViewModel> clienterViewModel { get; set; }
        public PagingResult PagingResult { get; set; }
    }

    public class ClienterManageList
    {
        public ClienterManageList(PagedList<clienter> _clienterModel, PagingResult pagingResult)
        {
            clienterModel = _clienterModel;
            _PagingResult = pagingResult;
        }
        public PagedList<clienter> clienterModel { get; set; }
        public PagingResult _PagingResult { get; set; }
    }
}
