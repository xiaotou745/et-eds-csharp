using System.Data;
using System.Text;
using ETS.Const;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Model.DomainModel.Bussiness;
using Ets.Service.IProvider.User;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using CalculateCommon;
using Ets.Model.ParameterModel.Bussiness;
using System.Linq;
using ETS.Enums;
using Ets.Model.DataModel.Bussiness;
using ETS.Util;
using ETS.Cacheing;
using Ets.Model.DataModel.Group;
using ETS.Validator;
using ETS;
using System.Threading.Tasks;
using ETS.Sms;
using ETS.Transaction.Common;
using ETS.Transaction;
using Ets.Model.ParameterModel.User;
using Ets.Model.ParameterModel.Order;
namespace Ets.Service.Provider.User
{
    /// <summary>
    /// 商家分组业务逻辑接口实现类 
    /// </summary>
    public class BusinessGroupProvider : IBusinessGroupProvider
    {        
        BusinessGroupDao dao = new BusinessGroupDao();

        /// <summary>
        /// 获取商家分组列表
        /// 胡灵波-20150504
        /// </summary>
        /// <returns></returns>
        public IList<BusinessGroupModel> GetBusinessGroupList()
        {
            return dao.GetBusinessGroupList();
        }

        public BusinessGroupModel GetCurrenBusinessGroup(int businessId)
        {
            return dao.GetCurrenBusinessGroup(businessId);
        }
    }
}