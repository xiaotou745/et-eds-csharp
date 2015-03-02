using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SuperMan.Models;
using SuperManCore;
using System.Web.ApplicationServices;
using SuperMan.Authority;
using SuperManBusinessLogic.Authority_Logic;
using SuperManCommonModel;
using System.Web.Security;
using System.Collections.Generic;
using SuperManDataAccess;
using SuperManBusinessLogic.CommonLogic;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class CommonController : Controller
    {
         /// <summary>
        /// 根据父级Code获取城市信息，默认FID为0，代表获取所有省   add by caoheyang 20150302
        /// </summary>
        /// <param name="Fid">父级Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRegionsByCode(string code = ConstValues.Code1) {
            return Json(RegionLogic.regionLogic().GetRegionsByCode(code), JsonRequestBehavior.DenyGet);
        }

    }
}