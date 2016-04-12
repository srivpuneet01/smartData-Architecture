using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Core.Domain;
using smartData.Models;
using smartData.Infrastructure;
using System.Dynamic;
using System.Web.Security;
using System.Web.Security;
using WebMatrix.WebData;
using Ninject;
using ServiceLayer.Interfaces;

namespace smartData
{
    [Authorize]
    public class PagePermissionController : Controller
    {
        //
        // GET: /PagePermission/PagePermission/

        #region Global Variables
        ServiceLayer.Interfaces.IPagePermissionService _pagePermissionService;
        IPagePermissionAPIController _pagePermissionAPIController;
        IActionAccessPermissionService _actionAccessPermissionService;
        public List<ScreenPermissionList> obj;
        #endregion

        #region constructor
        public PagePermissionController(IPagePermissionService _PagePermissionService, IActionAccessPermissionService actionAccessPermissionService, IPagePermissionAPIController _IPagePermissionAPIController)
        {
            _pagePermissionService = _PagePermissionService;
            _pagePermissionAPIController = _IPagePermissionAPIController;
            _actionAccessPermissionService = actionAccessPermissionService;
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Demo()
        {
            try
            {
                if ((Session["TokenID"].ToString()) == null || Session["TokenID"].ToString() == "")
                {


                    return RedirectToAction("SessionExpire", "Home", new { area = "" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SessionExpire", "Home", new { area = "" });
            }
            var TokenID = Session["TokenID"].ToString();
           // ServiceLayer.Services.ActionAccessPermissionService _ActionAccessPermissionService = new ServiceLayer.Services.ActionAccessPermissionService();
            var UID = TokenID;
            obj = _actionAccessPermissionService.GetScreenPermissionByUserIdAndScreenId((UID), "Demo");
            //TODO update LastActivity Time in Request
            _actionAccessPermissionService.UpdateLastActivityTime(TokenID);



          
            if (obj.Count == 0)
            {
                _actionAccessPermissionService.UpdateLoginAuthentication(UID);
                return RedirectToAction("SessionExpire", "Home", new { area = "" });
            }
            else
            {
                if ((obj.Where(x => x.ActionType == 1).ToList()).Count > 0)
                {
                    return View(obj);

                }
                else
                {

                    // return View(,"Accessdenied.cshtml");
                    return View("Accessdenied");
                }
            }
        }
        public ActionResult Accessdenied()
        {
            return View();
        }
        [HttpPost, ActionName("GetAccessPermissionList")]
        public JsonResult GetAccessPermissionList(string Screen)
        {
          //  ServiceLayer.Services.ActionAccessPermissionService _ActionAccessPermissionService = new ServiceLayer.Services.ActionAccessPermissionService();
            var UID = Session["TokenID"].ToString();//Membership.GetUser().ProviderUserKey.ToString();
            var data = _actionAccessPermissionService.GetScreenPermissionByUserIdAndScreenId((UID), Screen);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpPost, ActionName("AddDemo")]
        public JsonResult AddDemo(String Screen)
        {
            var data = "0";
            try
            {

                data = _pagePermissionService.GetAllowPermission(Session["TokenID"].ToString(), Screen, "Add");
                if (data == "1")
                {

                }
            }
            catch (Exception ex)
            { return Json(data, JsonRequestBehavior.AllowGet); }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //TODO: Action Result for NewScreen
        public ActionResult newScreen112()
        {
            try
            {
                if ((Session["TokenID"].ToString()) == null || Session["TokenID"].ToString() == "")
                {


                    return RedirectToAction("SessionExpire", "Home", new { area = "" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SessionExpire", "Home", new { area = "" });
            }
            var TokenID = Session["TokenID"].ToString();
           // ServiceLayer.Services.ActionAccessPermissionService _ActionAccessPermissionService = new ServiceLayer.Services.ActionAccessPermissionService();
            var UID = TokenID;
            obj = _actionAccessPermissionService.GetScreenPermissionByUserIdAndScreenId((UID), "new Screen112");
            //  obj.Where(x => x.ActionType == 1).ToList();rest Demo
            if (obj.Count == 0)
            {
                _actionAccessPermissionService.UpdateLoginAuthentication(UID);
                return RedirectToAction("SessionExpire", "Home", new { area = "" });
            }
            else
            {
                if ((obj.Where(x => x.ActionType == 1).ToList()).Count > 0)
                {
                    return View(obj);

                }
                else
                {

                    // return View(,"Accessdenied.cshtml");
                    return View("Accessdenied");
                }
            }
        }
    }
}