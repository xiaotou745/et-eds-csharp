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
using Ets.Service.Provider.User;
using Ets.Service.IProvider.User; 

namespace OpenApi.Controllers
{
    public class BusinessController : ApiController
    {
        IBusinessProvider iBusiProvider = new BusinessProvider();
        /// <summary>
        /// 商户注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        [OpenApiActionError]
        public ResultModel<object> RegisterBusiness(ParaModel<BusinessRegisterModel> paramodel)
        { 
            //是否存在该商户
            if (iBusiProvider.CheckExistBusiness(paramodel.fields.B_OriginalBusiId, paramodel.group))
                return ResultModel<object>.Conclude(CustomerRegisterStatus.OriginalBusiIdRepeat); 

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