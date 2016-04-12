using smartData.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smartData.Controllers
{
     [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = CustomMessages.YourAppDescPage;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = CustomMessages.YourContactPage;

            return View();
        }

        public ActionResult SessionExpire()
        {
            ViewBag.Message = CustomMessages.YourSessionExpired;

            return View();
        }

        public ActionResult NoPermission()
        {
            ViewBag.Message = CustomMessages.NotAuthorizedToView;

            return View();
        }
	}
}