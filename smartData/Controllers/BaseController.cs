using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace smartData.Controllers
{
    public class BaseController : Controller
    {

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.IsInRole("admin"))
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Users", action = "index" }));
        }
    }
}