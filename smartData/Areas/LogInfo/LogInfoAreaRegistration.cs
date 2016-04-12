using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smartData.Areas.LogInfo
{
    public class LogInfoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LogInfo";
            }
        }
          public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LogInfo_default",
                "LogInfo/{controller}/{action}/{id}",
              new { controller = "LogInfo", action = "LogIndex", id = UrlParameter.Optional },
                 namespaces: new string[] { "smartData.Areas.LogInfo.Controllers" }
            );
        }
    }
}

      
