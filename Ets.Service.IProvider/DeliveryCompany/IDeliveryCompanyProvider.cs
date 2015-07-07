using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.DeliveryCompany;

namespace Ets.Service.IProvider.DeliveryCompany
{
    public interface IDeliveryCompanyProvider
    {
        IList<DeliveryCompanyModel> Get();
    }
}
