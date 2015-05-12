using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using ETS.Util;
using Ets.Model.Common;
using ETS.Enums;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Finance;
namespace SuperManWebApi.Controllers
{ 
    /// <summary>
    /// 商户相关接口 add by caoheyang
    /// </summary>
    public class BusinessController : ApiController
    {
        private  readonly  IBusinessFinanceProvider _businessFinanceProvider=new BusinessFinanceProvider();        

        /// <summary>
        /// 商户交易流水API caoheyang 20150512
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<IList<FinanceRecordsDM>> Records()
        {
            int businessId = ParseHelper.ToInt(HttpContext.Current.Request.Form["businessId"]);
            return _businessFinanceProvider.GetRecords(businessId);
        }

        /// <summary>
        /// 商户详情        
        /// </summary>
        /// <param name="model">商户参数</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<BusinessDM> GetDetails(BussinessPM model)
        {
            //加验证

            BusinessDM businessDM = _businessFinanceProvider.GetDetails(model.BussinessId);
            return Ets.Model.Common.ResultModel<BusinessDM>.Conclude(GetOrdersStatus.Success, businessDM);
        }
    }
}
