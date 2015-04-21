using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Clienter;
using Ets.Service.Provider.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.ComponentModel.DataAnnotations; 
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Bussiness;
using ETS.Enums;

namespace OpenApi.Controllers
{
    public class BusinessController : ApiController
    { 
        /// <summary>
        /// 商户注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        [OpenApiActionError]
        public ResultModel<object> RegisterBusiness(ParaModel<BusinessRegisterModel> paramodel)
        {
            if (paramodel.app_key.Length > 0)
            {
                return ResultModel<object>.Conclude(BusiRegisterStatusType.Success);
            }
            else
            {
                return ResultModel<object>.Conclude(BusiRegisterStatusType.ParaError);
            }
        }
    }
}