using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using ETS.Util;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Clienter;
using Ets.Service.IProvider.Clienter;
using ETS.Enums;
namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 骑士相关接口 add by caoheyang
    /// </summary>
    public class ClienterController : ApiController
    {
        IClienterFinanceProvider _iClienterFinanceProvider=new ClienterFinanceProvider();
        IClienterProvider _iClienterProvider = new ClienterProvider();
        /// <summary>
        /// 骑士交易流水API
        /// </summary>
        /// <returns></returns>
       [HttpPost]
        public IList<ClienterBalanceRecord> Records()
        {
            int clineterId = ParseHelper.ToInt(HttpContext.Current.Request.Form["clineterId"]);
            return _iClienterFinanceProvider.GetRecords(clineterId);
        }

       /// <summary>
       /// 骑士详情        
       /// </summary>
       /// <param name="model">骑士参数</param>
       /// <returns></returns>        
       [HttpPost]
       public ResultModel<ClienterDM> GetDetails(ClienterPM model)
       {
           //加验证

           ClienterDM clienterDM = _iClienterProvider.GetDetails(model.ClienterId);
           return Ets.Model.Common.ResultModel<ClienterDM>.Conclude(GetOrdersStatus.Success, clienterDM);
       }
    }
}
