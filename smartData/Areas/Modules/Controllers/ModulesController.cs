
using smartData.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreEntities.Domain;
using Core.Domain;
using CoreEntities.CustomModels;
using WebMatrix.WebData;
using ServiceLayer.Services;
using ServiceLayer.Interfaces;

namespace smartData
{
     [Authorize]
    public class ModulesController : Controller
    {

          #region Global Variables

        
        //
        // GET: /Roles/Roles/
         ServiceLayer.Interfaces.IModuleService _moduleService;
         ServiceLayer.Interfaces.IUserService _userService = null;
         IModuleAPIController _moduleAPIController;
        #endregion

          #region constructor

        public ModulesController(IModuleService moduleService, IUserService userService, IModuleAPIController moduleAPIController)
        {
            _moduleService = moduleService;
            _userService = userService;
            _moduleAPIController = moduleAPIController;
        }
        #endregion
        //
        // GET: /Modules/Modules/
        public ActionResult Index()
        {
            long UID = Convert.ToInt64(WebSecurity.CurrentUserId);           
            bool lastResult = _userService.IsUserPermitedtoDelete(UID);
            if (lastResult == false)
            {
                return View("NoPermissionUser");
            }
            else
            {
                ViewBag.logUser = WebSecurity.CurrentUserId;
                return View();
            }
        }

         public ActionResult CreateModule()
        {
            return View();
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         [HandleException]
         public dynamic Create([Bind(Include = "ModuleID,ModuleName,CreatedBy")] Module module)
         {
             //  _iRolesAPIController = new RolesAPIController();
             if (ModelState.IsValid)
             {
                 // role.Active = true;
                 ResponseModel _result = _moduleAPIController.CreateModule(module);
                 if (_result.ResponseStatus)
                 {
                     //return RedirectToAction("Index");
                     return this.Json(_result, JsonRequestBehavior.DenyGet);
                 }
                 else
                 {
                     if (Request.IsAjaxRequest())
                     {
                         return this.Json(_result, JsonRequestBehavior.DenyGet);
                     }
                     else
                     {
                         return View(module);
                     }
                 }
             }
             return View(module);
         }

         [HttpPost]
         [ActionName("EditModule")]
        // [AntiforgeryValidate]
         public void EditModule(Module module)
         {
             _moduleService.EditModule(module);
         }

         //SumitS
         public ActionResult NoPermissionUser()
         {
             ViewBag.Message = "You are not authorized to view this page.";

             return View();
         }
	}
}