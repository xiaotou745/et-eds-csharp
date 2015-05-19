using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.Provider.Common;

namespace SuperManWebApi.App_Start.Filters
{
    /// <summary>
    /// 对内接口的参数合法性验证 add by caoheyang 20150512
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ApiVersionAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var verSion = HttpContext.Current.Request.Form["version"];
            if (!string.IsNullOrWhiteSpace(verSion))
            {
                Task.Factory.StartNew(() =>
                {
                    var model = new ApiVersionStatisticModel
                    {
                        APIName = actionContext.Request.RequestUri.AbsolutePath,
                        CreateTime = DateTime.Now,
                        Version = verSion
                    };
                    new ApiVersionProvider().AddApiRecords(model);
                });
            }
        }


    }
}