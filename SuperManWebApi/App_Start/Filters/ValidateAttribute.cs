using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using ETS.Enums;
using Ets.Model.Common;

namespace SuperManWebApi.App_Start.Filters
{
    /// <summary>
    /// 对内接口的参数合法性验证 add by caoheyang 20150512
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// 重写OnActionExecuting方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                IList<string> errors = new List<string>();
                foreach (string key in actionContext.ModelState.Keys)
                {
                    foreach (ModelError t in actionContext.ModelState[key].Errors)
                    {
                        errors.Add(t.ErrorMessage);
                    }
                }
                actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                    (actionContext.ControllerContext, ResultModel<object>.Conclude(SystemEnum.ParaError, errors));
            }
        }

    }
}