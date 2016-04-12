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
using WebMatrix.WebData;
using ServiceLayer.Interfaces;

namespace smartData
{//Areas.Roles.Controllers
    [Authorize]
    public class RolesController : Controller
    {
        #region Global Variables

        
        //
        // GET: /Roles/Roles/
        ServiceLayer.Interfaces.IRolesService _roleService;
        ServiceLayer.Interfaces.IUserService _userService=null;
        IRolesAPIController _rolesAPIController;
        #endregion

        #region constructor
       
        public RolesController(IRolesService rolesService, IUserService userService, IRolesAPIController rolesAPIController)
        {
            _roleService = rolesService;
            _userService = userService;
            _rolesAPIController = rolesAPIController;
        }
        #endregion

        #region  Public Methods
       
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
        public ActionResult Create()
        {
            return View();
        }
        // POST: /Users/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [HandleException]
        public dynamic Create([Bind(Include = "RoleId,RoleName,Active")] Roles role)
        {
            //  _iRolesAPIController = new RolesAPIController();
            if (ModelState.IsValid)
            {
                // role.Active = true;
                ResponseModel _result = _rolesAPIController.CreateRole(role);
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
                        return View(role);
                    }
                }
            }
            return View(role);
        }

        [HttpPost]
        [ActionName("MassUpdate")]
        public JsonResult MassUpdate(string roleNames, string UnCheckIds, int ModifyBy)
        {
            var v = "";
            _roleService.MassUpdate(roleNames, UnCheckIds, ModifyBy);
            return Json(v, JsonRequestBehavior.AllowGet);

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