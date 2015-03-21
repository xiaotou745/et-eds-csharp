using Ets.Service.Provider.Common;
using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SuperManWebApi.Controllers
{
    public class CommonController : ApiController
    {

        /// <summary>
        /// 获取开通城市的省市区
        /// 窦海超
        /// 2015年3月16日 11:44:54
        /// </summary>
        /// <param name="Version">版本号</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.CityStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Area.AreaModelList> GetOpenCity(string Version)
        {
            AreaProvider area = new AreaProvider();
            return area.GetOpenCity(Version);
        }
    }
}
