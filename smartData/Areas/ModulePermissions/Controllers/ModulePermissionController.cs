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
    public class ModulePermissionController : Controller
    {
        #region Global Variables

        //  GET: /ModulePermissions/ModulePermission/
        ServiceLayer.Interfaces.IScreenPermissionService _screenPermissionService;
        ServiceLayer.Interfaces.IModulePermissionService _modulePermissionService;
        ServiceLayer.Interfaces.IUserService _userService = null;
        // IRolesAPIController _iRolesAPIController;
        #endregion

        #region constructor

        public ModulePermissionController(IUserService userService, IScreenPermissionService screenPermissionService, IModulePermissionService modulePermissionService)
        {
            _userService = userService;// new UserService();
            _screenPermissionService = screenPermissionService;// new ScreenPermissionService();
            _modulePermissionService = modulePermissionService;// new ModulePermissionService();
        }

        #endregion

        #region  Public Methods

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AddModulePermission()
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

        [HttpPost]
        [ActionName("GetAllScreenPermission")]
        public JsonResult GetAllScreenPermission(int ModuleID, int RoleId)
        {

            //var v = mm.Students.Where(m => m.RollNo == i).Select(m => new { RollNo = m.RollNo, Name = m.Name, Course = m.Course, Gender = m.Gender }).FirstOrDefault();
            var v = _modulePermissionService.GetAllScreenPermission(ModuleID,RoleId);
            return Json(v, JsonRequestBehavior.AllowGet);

        }
        

        [HttpPost]
        [ActionName("CreateFullPermission")]
        [HandleAPIExceptionAttribute]
        public JsonResult CreateFullPermission(int ModuleId, int RoleId, bool FullPermission)
        {
            ModulePermission modulepermission = new ModulePermission();
            modulepermission.ModuleId = ModuleId;
            modulepermission.RoleId = RoleId;
            modulepermission.FullPermission = FullPermission;
            ResponseModel _result = _modulePermissionService.CreateFullPermission(modulepermission);
            return Json(_result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("GetModulePermission")]
        [HandleAPIExceptionAttribute]
        public JsonResult GetModulePermission(int ModuleId, int RoleId)
        {
            var data = _modulePermissionService.GetModulePermission(ModuleId, RoleId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionName("DeleteModulePermission")]
        [HandleAPIExceptionAttribute]
        public JsonResult DeleteModulePermission(int ModuleID, int RoleId)
        {

            var data = _modulePermissionService.DeleteModulePermission(ModuleID, RoleId);
          return Json(data, JsonRequestBehavior.AllowGet);
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