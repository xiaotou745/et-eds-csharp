using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Model.DomainModel.DeliveryCompany;
using Ets.Model.ParameterModel.DeliveryCompany;
using ETS.Data.PageData;

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
        /// <summary>
        /// 物流公司批量导入骑士  add by caoheyang 20150707
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <param name="models">骑士集合</param>
        /// <returns></returns>
        ResultModel<object> DoBatchImportClienter(int companyId, List<BatchImportClienterExcelDM> models);

        PageInfo<DeliveryCompanyModel> Get(DeliveryCompanyCriteria deliveryCompanyCriteria);

        ResultModel<DeliveryCompanyResultModel> Add(DeliveryCompanyModel deliveryCompanyModel);
    }
}
