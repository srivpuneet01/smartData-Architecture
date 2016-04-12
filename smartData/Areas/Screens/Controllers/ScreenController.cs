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
using CoreEntities.Domain;
using CoreEntities.CustomModels;
using WebMatrix.WebData;
using ServiceLayer.Interfaces;

namespace smartData.Areas.screens.Controllers
{
    [Authorize]
    public class screenController : Controller
    {
        #region Global Variables

        IScreenService _screenService;
        IUserService _userService = null;
        IScreenAPIController _screenAPIController;
        #endregion

        #region constructor
        public screenController(IScreenService screenService, IUserService userService, IScreenAPIController screenAPIController)
        {
            _screenService = screenService;
            _userService = userService;
            _screenAPIController = screenAPIController;
        }
        #endregion

        #region  Public Methods

        public ActionResult Index()
        {

            ViewBag.logUser = WebSecurity.CurrentUserId;
            Screen model = new Screen();
            IEnumerable<ModelType> actionTypes = Enum.GetValues(typeof(ModelType))
                                                       .Cast<ModelType>();
            model.ModuleIdList = from action in actionTypes
                                 select new SelectListItem
                                 {
                                     Text = action.ToString(),
                                     Value = ((int)action).ToString(),

                                 };
            

            ViewBag.ActionType = model.ModuleIdList;
            model = _screenService.GetDropDown();
            //  return View(obj);
            ViewBag.moduleList = model.ModuleList; //model.ModuleIdList;

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
        public ActionResult Create()
        {

            return View();
        }
        // POST: /Users/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        [HandleException]
        public ActionResult Create([Bind(Include = "screenId,ModuleId,ScreenName,CreatedBy")] Screen screen)
        {
            ResponseModel _result = new ResponseModel();
            //  _iRolesAPIController = new RolesAPIController();
            if (ModelState.IsValid)
            {
                // role.Active = true;

                _result = _screenAPIController.Createscreen(screen);
                //  return RedirectToAction("Index");
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
                        return View(screen);
                    }
                }

            }
            else
            {
                _result.ResponseStatus = false;
                _result.ResponseText = "Please select valid module.";
                return this.Json(_result, JsonRequestBehavior.DenyGet);
            }
            return View(screen);
        }

        //Add ScreenAction
        [HandleException]
        public ActionResult ScreenAction([Bind(Include = "ActionId,FK_screenId,ActionName,ActionType")] ScreenAction screenAction)
        {
            //  _iRolesAPIController = new RolesAPIController();
            // if (ModelState.IsValid)
            {


                string action = ((ModelType)screenAction.ActionType).ToString();
                int value;
                if (int.TryParse(action, out value))
                {
                    ResponseModel _result1 = new ResponseModel();

                    _result1.ResponseStatus = false;
                    _result1.ResponseText = "Sorry, Action Type has some error.";
                    return this.Json(_result1, JsonRequestBehavior.DenyGet);
                }


                // role.Active = true;
                ResponseModel _result = _screenAPIController.AddScreenAction(screenAction);
                //  return RedirectToAction("Index");
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
                        return View(screenAction);
                    }
                }
            }
            return View(screenAction);
        }
        [HttpGet]
        public JsonResult showGridlIst(int FK_screenId)
        {

            //var v = mm.Students.Where(m => m.RollNo == i).Select(m => new { RollNo = m.RollNo, Name = m.Name, Course = m.Course, Gender = m.Gender }).FirstOrDefault();
            var v = _screenService.GetAllScreenActionByScreeiID(FK_screenId);
            return Json(v, JsonRequestBehavior.AllowGet);

        }
        //  [ValidateAntiForgeryToken]
        [HttpPost]
        // [HandleException]
        public ActionResult EditScreenAction(ScreenAction screenaction)
        {
            try
            {
                string action = ((ModelType)screenaction.ActionType).ToString();
                int value;
                if (int.TryParse(action, out value))
                {
                    return RedirectToAction("Index");
                }

                _screenAPIController.EditScreenAction(screenaction);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult DeleteScreenAction(int id)
        {
            try
            {

                bool data = _screenAPIController.DeleteScreenAction(id);
                //  return RedirectToAction("Index");
                // }
                if (Request.IsAjaxRequest())
                {
                    return this.Json(data, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Test()
        {

            return View();
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