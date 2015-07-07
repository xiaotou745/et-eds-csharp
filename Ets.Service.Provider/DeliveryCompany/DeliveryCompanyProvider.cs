using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.DeliveryCompany;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Service.IProvider.DeliveryCompany;

namespace Ets.Service.Provider.DeliveryCompany
{
    public class DeliveryCompanyProvider : IDeliveryCompanyProvider
    {
        readonly DeliveryCompanyDao dao=new DeliveryCompanyDao();
        /// <summary>
        /// 获取物流公司列表
        /// danny-20150706
        /// </summary>
        /// <returns></returns>
        public IList<DeliveryCompanyModel> GetDeliveryCompanyList()
        {
            return dao.GetDeliveryCompanyList();
        }
    }
}
