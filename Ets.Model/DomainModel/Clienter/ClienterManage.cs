using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Clienter
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
        public ClienterCountManageList(List<ClienterViewModel> _clienterViewModel, NewPagingResult pagingResult)
        {
            clienterViewModel = _clienterViewModel;
            PagingResult = pagingResult;
        }
        public List<ClienterViewModel> clienterViewModel { get; set; }
        public NewPagingResult PagingResult { get; set; }
    }

    public class ClienterManageList
    {
        public ClienterManageList(List<ClienterListModel> _clienterModel, NewPagingResult pagingResult)
        {
            clienterModel = _clienterModel;
            _PagingResult = pagingResult;
        }
        public List<ClienterListModel> clienterModel { get; set; }
        public NewPagingResult _PagingResult { get; set; }
    }
}

