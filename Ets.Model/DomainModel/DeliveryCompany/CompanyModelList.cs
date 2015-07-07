using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.DeliveryCompany
{
    public class CompanyModelList
    {
        public IList<CompanyModel> CompanyModels { get; set; }
        public string Version { get; set; }
    }
}
