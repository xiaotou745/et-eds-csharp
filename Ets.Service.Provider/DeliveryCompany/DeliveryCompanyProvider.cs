using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.DomainModel.DeliveryCompany;
using Ets.Service.IProvider.DeliveryCompany;

namespace Ets.Service.Provider.DeliveryCompany
{
    /// <summary>
    /// 物流公司业务逻辑
    /// </summary>
    public class DeliveryCompanyProvider : IDeliveryCompanyProvider
    {
        /// <summary>
        /// 物流公司批量导入骑士  add by caoheyang 20150707
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <param name="models">骑士集合</param>
        /// <returns></returns>
        public ResultModel<object> DoBatchImportClienter(int companyId, List<BatchImportClienterExcelDM> models)
        {
            return null;
        }
    }
}
