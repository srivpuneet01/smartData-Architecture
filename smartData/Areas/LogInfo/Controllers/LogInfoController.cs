using ServiceLayer.Interfaces;
using System.Web.Mvc;

namespace smartData.Areas.LogInfo.Controllers
{
    [Authorize]
    public class LogInfoController : Controller
    {
        
        ILogInfoService _loginfoService;
        ILogInfoAPIController _loginfoAPIController;
        string str = "";
        public LogInfoController(ILogInfoService logInfoService, ILogInfoAPIController logInfoAPIController)
        {
            _loginfoService = logInfoService;
            _loginfoAPIController = logInfoAPIController;
            if (System.Web.HttpContext.Current.Request.UrlReferrer != null && System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri != null)
            {
                str = System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString();
                string Fullurl = str + "api/PatientsAPI/GetPatients";
                ViewBag.TempUrl = Fullurl;
            }
        }  
        // GET: /LogInfo/LogInfo/
        public ActionResult LogIndex()
        {          
            return View();
        }

    }
}