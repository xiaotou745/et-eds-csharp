using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Common;
using Ets.Dao.DeliveryCompany;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Service.IProvider.DeliveryCompany;

namespace Ets.Service.Provider.DeliveryCompany
{
    public class DeliveryCompanyProvider : IDeliveryCompanyProvider
    {
        DeliveryCompanyDao dcd = new DeliveryCompanyDao();
        /// <summary>
        /// 获取物流公司
        /// </summary>
        /// <returns></returns>
        public IList<DeliveryCompanyModel> Get()
        {
            return null;
        }

    }
}
