using smartData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using System.Globalization;

namespace smartData
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebSecurity.InitializeDatabaseConnection("AzureDBContext", "Users", "UserId", "Email", true);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            //if (HttpContext.Current.User.IsInRole("admin"))
            //{
            //    // return RedirectToRoute("/elmah.axd");
            //}
        }
        protected void Application_BeginRequest()
        {
            CultureInfo info = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            info.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = info;
        }
    }
}
