using System.Web.Mvc;

namespace smartData.Areas.ModulePermissions
{
    public class ModulePermissionsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ModulePermissions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ModulePermissions_default",
                "ModulePermissions/{controller}/{action}/{id}",
                //new { action = "Index", id = UrlParameter.Optional }
                 new { controller = "ModulePermission", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "smartData" }
            );
        }
    }
}