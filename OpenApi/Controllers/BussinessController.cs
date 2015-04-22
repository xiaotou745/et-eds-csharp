using Ets.Model.Common;
using Ets.Model.ParameterModel.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenApi.Controllers
{
    public class BussinessController : Controller
    {
        [HttpPost]
        [SignOpenApi] //sign验证过滤器 设计参数验证，sign验证 add by caoheyang 20150316
        [OpenApiActionError] //异常过滤器 add by caoheyang 一旦发生异常，客户端返回系统内部错误提示
        public ResultModel<object> RegisterBussiness(ParaModel<NewRegisterInfoModel> paramodel)
        {

            return null;
        }
    }
}