using Ets.Model.DomainModel.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
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
}
