using System.Web.Mvc;
using admin.edaisong.com.App_Start;

namespace admin.edaisong.com.Controllers
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