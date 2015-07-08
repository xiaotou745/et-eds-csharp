using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.DeliveryCompany;
using Ets.Model.Common;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Model.DomainModel.DeliveryCompany;
using Ets.Model.ParameterModel.DeliveryCompany;
using Ets.Service.IProvider.DeliveryCompany;
using ETS.Data;
using ETS.Data.PageData;
using ETS.Enums;

namespace Ets.Service.Provider.DeliveryCompany
{
    /// <summary>
    /// 物流公司业务逻辑
    /// </summary>
    public class DeliveryCompanyProvider : IDeliveryCompanyProvider
    {
        readonly DeliveryCompanyDao dao = new DeliveryCompanyDao();
        /// <summary>
        /// 获取物流公司列表
        /// danny-20150706
        /// </summary>
        /// <returns></returns>
        public IList<DeliveryCompanyModel> GetDeliveryCompanyList()
        {
            return dao.GetDeliveryCompanyList();
        }

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
        /// <summary>
        /// 获取物流公司
        /// </summary>
        /// <returns></returns>
        public PageInfo<DeliveryCompanyModel> Get(DeliveryCompanyCriteria deliveryCompanyCriteria)
        {
            return dao.Get<DeliveryCompanyModel>(deliveryCompanyCriteria);
        }

        public DeliveryCompanyModel GetById(int Id)
        {
            return dao.GetById(Id);
        }
        public ResultModel<DeliveryCompanyResultModel> Add(DeliveryCompanyModel deliveryCompanyModel)
        {
            int addId = dao.Add(deliveryCompanyModel);
            DeliveryCompanyResultModel dcrm = new DeliveryCompanyResultModel();

            if (addId > 0)
            {
                dcrm.Id = addId;
                return ResultModel<DeliveryCompanyResultModel>.Conclude(DeliveryStatus.Success, dcrm);
            }
            else
            {
                return ResultModel<DeliveryCompanyResultModel>.Conclude(DeliveryStatus.Fail, null);
            }
        }

        public ResultModel<DeliveryCompanyResultModel> Modify(DeliveryCompanyModel deliveryCompanyModel)
        {
            int modifyResult = dao.Modify(deliveryCompanyModel);
            DeliveryCompanyResultModel dcrm = new DeliveryCompanyResultModel();

            if (modifyResult > 0)
            { 
                return ResultModel<DeliveryCompanyResultModel>.Conclude(DeliveryStatus.Success, null);
            }
            else
            {
                return ResultModel<DeliveryCompanyResultModel>.Conclude(DeliveryStatus.Fail, null);
            }
        }
    }
}
