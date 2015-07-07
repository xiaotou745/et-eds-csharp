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
        /// <summary>
        /// 获取物流公司列表
        /// danny-20150706
        /// </summary>
        /// <returns></returns>
        IList<DeliveryCompanyModel> GetDeliveryCompanyList();
    }
}
