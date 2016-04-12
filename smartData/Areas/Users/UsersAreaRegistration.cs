using System.Web.Mvc;

namespace smartData.Areas.Users
{
    public class UsersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Users";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Users_elmah",
                "Users/elmah/{type}",
                new { action = "Index", controller = "Elmah", type = UrlParameter.Optional });

            context.MapRoute(
                "Users_elmahdetail",
                "Users/elmah/detail/{type}",
                new { action = "Index", controller = "Elmah", type = UrlParameter.Optional }
        );
            context.MapRoute(
                            "Users_elmahabout",
                            "Users/elmah/about/{type}",
                            new { action = "Index", controller = "Elmah", type = UrlParameter.Optional }
                    );
            context.MapRoute(
                "Users_elmahdetailabout",
                "Users/elmah/detail/about/{type}",
                new { action = "Index", controller = "Elmah", type = UrlParameter.Optional }
        );
            context.MapRoute(
    "Users_default",
    "Users/{controller}/{action}/{id}",
    new { controller = "Users", action = "Index", id = UrlParameter.Optional },
    namespaces: new string[] { "smartData.Areas.Users.Controllers" }
);

        }
    }
}