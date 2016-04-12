using System.Web.Mvc;

namespace smartData.Areas.Roles
{
    public class RolesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Roles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Roles_default",
                "Roles/{controller}/{action}/{id}",
                //new { action = "Index", id = UrlParameter.Optional }
                new { controller = "Roles", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "smartData" }
                //smartData.Areas.Roles.Controllers
            );
        }
    }
}