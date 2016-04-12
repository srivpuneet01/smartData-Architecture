using System.Web.Mvc;

namespace smartData.Areas.Modules
{
    public class ModulesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Modules";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Modules_default",
                "Modules/{controller}/{action}/{id}",
                //new { action = "Index", id = UrlParameter.Optional }
                 new { controller = "Modules", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "smartData" }
            );
        }
    }
}