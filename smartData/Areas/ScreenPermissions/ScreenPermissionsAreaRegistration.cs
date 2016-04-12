using System.Web.Mvc;

namespace smartData.Areas.ScreenPermissions
{
    public class ScreenPermissionsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ScreenPermissions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ScreenPermissions_default",
                "ScreenPermissions/{controller}/{action}/{id}",
               // new { action = "Index", id = UrlParameter.Optional }
                 new { controller = "ScreenPermissions", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "smartData" }
            );
        }
    }
}