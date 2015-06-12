using System.Web.Mvc;

namespace Eds.Web.Controller
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserContext.Current.Id == 0)
            {
                Response.Redirect("/account/login");
            }
        }
    }
}