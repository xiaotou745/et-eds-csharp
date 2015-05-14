using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SuperMan.Controllers.api
{
    /// <summary>
    /// 监控服务
    /// </summary>
    public class MonitoringController : ApiController
    {
        [HttpGet]
        public string Index()
        {
            return "success";
        }
    }
}
