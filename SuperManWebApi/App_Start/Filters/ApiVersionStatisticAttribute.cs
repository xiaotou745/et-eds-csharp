using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using Ets.Model.Common;
using Ets.Service.Provider.Common;

namespace SuperManWebApi.App_Start.Filters
{
    /// <summary>
    /// 用于接口统计--平扬.2015.4.14
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class ApiVersionStatisticAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// 重写OnActionExecuting方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Task.Factory.StartNew(() =>
            {
                var verSion = actionContext.ActionArguments["Version"] as string;
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