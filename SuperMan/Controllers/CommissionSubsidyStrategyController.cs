using System.Web.Mvc;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
namespace SuperMan.Controllers
{
    public class CommissionSubsidyStrategyController : BaseController
    {
        IBusinessGroupProvider iBusinessGroupProvide = new BusinessGroupProvider();
        // GET: CommissionSubsidyStrategy
        public ActionResult CommissionSubsidyStrategy()
        {
            ViewBag.StrategyList = iBusinessGroupProvide.GetBusinessGroupList();
            return View();
        }
    }
}