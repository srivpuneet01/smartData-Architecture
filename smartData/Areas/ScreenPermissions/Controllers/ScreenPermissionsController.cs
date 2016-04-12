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
using ServiceLayer.Services;
using CoreEntities.CustomModels;
using Newtonsoft.Json.Linq;
using WebMatrix.WebData;
using ServiceLayer.Interfaces;

namespace smartData
{
    public class ScreenPermissionsController : Controller
    {
        #region Global Variables

        //  GET: /ScreenPermissions/ScreenPermissions/
        ServiceLayer.Interfaces.IScreenPermissionService _screenPermissionService;
        ServiceLayer.Interfaces.IUserService _userService = null;
        // IRolesAPIController _iRolesAPIController;
        #endregion

        #region constructor
      
        public ScreenPermissionsController(IScreenPermissionService screenPermissionService, IUserService userService)
        {
            _screenPermissionService = screenPermissionService;
            _userService = userService;
        }

        #endregion

        #region  Public Methods
       
        public ActionResult Index()
        {

            ScreenPermission model = new ScreenPermission();
           // model.SceenList = _screenPermissionService.GetAllscreen();
           //ViewBag.SceenList = model1;

            model = _screenPermissionService.GetDropDown();
             long UID = Convert.ToInt64(WebSecurity.CurrentUserId);           
            bool lastResult = _userService.IsUserPermitedtoDelete(UID);
            if (lastResult == false)
            {
                return View("NoPermissionUser");
            }
            else
            {
                return View(model);
            }
        }
        //[HttpPost]
        //[ActionName("GetScreenPermissionList")]
       
        //public dynamic GetScreenPermissionList(JObject Obj)
        //{
        //    var model = _screenPermissionService.GetAllScreenPermission();
        //    return model;
        //}

        [HttpGet]
        [ActionName("GetScreenPermissionList")]
        public JsonResult GetScreenPermissionList()
        {

            //var v = mm.Students.Where(m => m.RollNo == i).Select(m => new { RollNo = m.RollNo, Name = m.Name, Course = m.Course, Gender = m.Gender }).FirstOrDefault();
            var v = _screenPermissionService.GetAllScreenPermission();
            return Json(v, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ActionName("AddScreenPermission")]
        [HandleAPIExceptionAttribute]
        public JsonResult AddScreenPermission(string str, int RoleId, int ModuleID)
        {
            var v = 1;
            _screenPermissionService.AddScreenPermission(str, RoleId, ModuleID);
            return Json(v, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("DeleteScreenPermission")]
        [HandleAPIExceptionAttribute]
        public JsonResult DeleteScreenPermission(string str, int RoleId, int ModuleID)
        {
            var v = "";
            _screenPermissionService.DeleteScreenPermission(str, RoleId, ModuleID);
            return Json(v, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("GetPermissionList")]
        public JsonResult GetPermissionList(int RoleId, int ModuleID)
        {
            //int RoleId=0, ModuleID=0;
            //var v = mm.Students.Where(m => m.RollNo == i).Select(m => new { RollNo = m.RollNo, Name = m.Name, Course = m.Course, Gender = m.Gender }).FirstOrDefault();
            var v = _screenPermissionService.GetPermissionList(RoleId, ModuleID);
            return Json(v, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        [ActionName("GetAllScreenPermissionl")]
        public JsonResult GetAllScreenPermissionl()
        {

            //var v = mm.Students.Where(m => m.RollNo == i).Select(m => new { RollNo = m.RollNo, Name = m.Name, Course = m.Course, Gender = m.Gender }).FirstOrDefault();
            var v = _screenPermissionService.GetAllScreenPermissionl();
            return Json(v, JsonRequestBehavior.AllowGet);

        }
        public ActionResult TestPage()
        {
            ScreenPermission model = new ScreenPermission();
            
            model = _screenPermissionService.GetDropDown();

            return View(model);
        }

        //SumitS
        public ActionResult NoPermissionUser()
        {
            ViewBag.Message = "You are not authorized to view this page.";

            return View();
        }
        #endregion
    }
}