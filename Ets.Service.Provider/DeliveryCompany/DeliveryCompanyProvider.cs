using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Clienter;
using Ets.Dao.DeliveryCompany;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.DeliveryCompany;
using Ets.Service.IProvider.DeliveryCompany;

namespace Ets.Service.Provider.DeliveryCompany
{
    /// <summary>
    /// 物流公司业务逻辑
    /// </summary>
    public class DeliveryCompanyProvider : IDeliveryCompanyProvider
    {
        readonly DeliveryCompanyDao dao = new DeliveryCompanyDao();
        private ClienterDao clienterDao = new ClienterDao();
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
            dynamic res = new ExpandoObject();
            res.InsertCount = 0; //成功数量
            res.UpdateCount = 0; //更新数量
            res.ErrorCount = 0; //失败数量
            res.ErrorPhones = new List<string>(); //失败数量
            foreach (BatchImportClienterExcelDM temp in models)
            {
                ClienterModel clienterInfo= clienterDao.GetUserInfoByUserPhoneNo(temp.Phone);
                if (clienterInfo == null) //骑士尚未存在，做insert操作。
                {
                    clienterDao.DeliveryCompanyInsertClienter(new clienter());
                    res.InsertCount = res.InsertCount + 1; //新增数量+1
                }
                else
                {
                    if (clienterInfo.TrueName == temp.Name && clienterInfo.IDCard == temp.IdCard) //做更新操作
                    {
                        res.UpdateCount = res.UpdateCount + clienterDao.BindDeliveryCompany(clienterInfo.Id, companyId); // 成功  更新数量+1
                    }
                    else
                    {
                        res.ErrorCount = res.ErrorCount + 1; //失败数量+1
                        res.ErrorPhones.Add(temp.Phone);
                    }
                }
            }
            return null;
        }
    }
}
