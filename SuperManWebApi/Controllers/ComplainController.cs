using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Complain;
using Ets.Model.ParameterModel.Complain;
using Ets.Service.IProvider.Complain;
using Ets.Service.Provider.Complain; 

namespace SuperManWebApi.Controllers
{
    
    /// <summary>
    /// 骑士和商家举报投诉相关
    /// </summary>
    [ExecuteTimeLog]
    public class ComplainController : ApiController
    {
        readonly IComplainProvider iComplainProvider = new ComplainProvider();
        /// <summary>
        /// 骑士举报商户
        /// </summary>
        /// <param name="complainParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<object> ClienterComplainBusiness([FromBody]ComplainParameter complainParameter)
        {
            ComplainModel complainModel = new ComplainModel()
            {
                ComplainId = complainParameter.ComplainId,
                ComplainedId = complainParameter.ComplainedId,
                OrderId =  complainParameter.OrderId,
                OrderNo = complainParameter.OrderNo,
                Reason = complainParameter.Reason.Replace(System.Environment.NewLine,""),
                ComplainType = ComplainTypeEnum.ClienterComplain.GetHashCode()
            };
            return iComplainProvider.Complain(complainModel);
        }

        /// <summary>
        /// 商户举报骑士
        /// </summary>
        /// <param name="complainParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<object> BusinessComplainClienter([FromBody]ComplainParameter complainParameter)
        {
            ComplainModel complainModel = new ComplainModel()
            {
                ComplainId = complainParameter.ComplainId,
                ComplainedId = complainParameter.ComplainedId,
                OrderId = complainParameter.OrderId,
                OrderNo = complainParameter.OrderNo,
                Reason = complainParameter.Reason,
                ComplainType = ComplainTypeEnum.BusinessComplain.GetHashCode()
            };
            return iComplainProvider.Complain(complainModel);
        }
    }
}