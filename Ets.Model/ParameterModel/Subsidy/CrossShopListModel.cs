using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Subsidy;
using Ets.Model.DomainModel.Clienter;

namespace Ets.Model.ParameterModel.Subsidy
{
    public class CrossShopListModel
    {
        public IList<CrossShopModel> list { get; set; }
        public ClienterModel ClienterModel { get; set; }
    }
}
