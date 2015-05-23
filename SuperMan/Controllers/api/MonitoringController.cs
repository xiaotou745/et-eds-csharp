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
    /// 给运维使用，
    /// 监控地址：http://myadmin.edaisong.com/api/monitoring
    /// 返回结果："success"
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
