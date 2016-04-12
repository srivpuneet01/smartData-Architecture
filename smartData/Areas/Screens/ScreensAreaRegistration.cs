using System.Web.Mvc;

namespace smartData.Areas.screens
{
    public class screensAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "screens";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "screens_default",
                "Screens/{controller}/{action}/{id}",
               // new { action = "Index", id = UrlParameter.Optional }
                 new { controller = "screen", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "smartData.Areas.screens.Controllers" }
                //smartData.Areas.Roles.Controllers
            );
        }
    }
}