using System.Web.Mvc;

namespace smartData.Areas.PagePermission
{
    public class PagePermissionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PagePermission";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PagePermission_default",
                "PagePermission/{controller}/{action}/{id}",
              //  new { action = "Index", id = UrlParameter.Optional }

                  new { controller = "PagePermission", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "smartData" }
            );
        }
    }
}