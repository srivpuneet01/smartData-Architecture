using System.Web.Mvc;

namespace smartData.Areas.RolePermissions
{
    public class RolePermissionsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RolePermissions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RolePermissions_default",
                "RolePermissions/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}