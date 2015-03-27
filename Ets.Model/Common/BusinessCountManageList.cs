using Ets.Model.DomainModel.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class BusinessCountManageList
    {
        public BusinessCountManageList(List<BusinessViewModel> _businessModel, NewPagingResult pagingResult)
        {
            businessModel = _businessModel;
            PagingResult = pagingResult;
        }
        public List<BusinessViewModel> businessModel { get; set; }
        public NewPagingResult PagingResult { get; set; }
    }
}
