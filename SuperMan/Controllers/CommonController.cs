using System.Web.Mvc;


namespace SuperMan.Controllers
{
    [WebHandleError]
    public class CommonController : BaseController
    {
         /// <summary>
        /// 根据父级Code获取城市信息，默认FID为0，代表获取所有省   add by caoheyang 20150302
        /// </summary>
        /// <param name="Fid">父级Id</param>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult GetRegionsByCode(string code = ConstValues.Code1) {
        //    return Json(RegionLogic.regionLogic().GetRegionsByCode(code), JsonRequestBehavior.DenyGet);
        //}

    }
}