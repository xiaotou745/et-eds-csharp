using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc.Async;
using Ets.Model.Common;
using Ets.Service.Provider.Common;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using ETS.Util;

namespace SuperManWebApi
{
    public class FilterConfig
    {
        /// <summary>
        /// 注册全局过滤器 add by caoheyang 20150205
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

  
    

  
}
