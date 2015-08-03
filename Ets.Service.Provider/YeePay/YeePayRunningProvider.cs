using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.YeePay;
using Ets.Model.Common;
using Ets.Model.DataModel.YeePay;
using Ets.Model.DomainModel.DeliveryCompany;
using Ets.Model.ParameterModel.YeePay;
using Ets.Service.IProvider.YeePay;
using ETS.Data;
using ETS.Data.PageData;
using ETS.Enums;

namespace Ets.Service.Provider.YeePay
{
    public class YeePayRunningProvider : IYeePayRunningProvider
    {
        readonly YeePayRunningDao dao = new YeePayRunningDao();

        public PageInfo<YeePayRunningAccountModel> Get(YeePayRunningCriteria yeePayRunningCriteria)
        {
            return dao.Get<YeePayRunningAccountModel>(yeePayRunningCriteria);
        }

        public IList<YeePayRunningAccountModel> GetYeePayRunningAccountList()
        {
            return dao.GetYeePayRunningAccountList();
        }

        public ResultModel<object> Add(YeePayRunningAccountModel yeePayRunningAccountModel)
        {
            int addId = dao.Add(yeePayRunningAccountModel);
             
            if (addId > 0)
            { 
                return ResultModel<object>.Conclude(YeePayRechargeStatus.Success, addId);
            }
            else
            {
                return ResultModel<object>.Conclude(YeePayRechargeStatus.Fail, null);
            }
        }
    }
}
