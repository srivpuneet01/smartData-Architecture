using Core.Domain;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using smartData.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebMatrix.WebData;
namespace smartData.Areas.Users.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        #region Global Variables
        ServiceLayer.Interfaces.IUserService _userService;
        IUsersAPIController _usersAPIController;
        public List<ScreenPermissionList> obj;
        public List<ScreenPermissionList> objScreenPermissionList = null;
        #endregion

        #region constructor
        public UsersController(UserService userService, IUsersAPIController usersAPIController)
        {
            _userService = userService;
            _usersAPIController = usersAPIController;
        }
        #endregion
        // GET: /Users/Users/

        #region  Public Methods

        public ActionResult Index()
        {
            ViewData["AllRole"] = _userService.GetAllRoles();
            // var User = new User();
            long UID = Convert.ToInt64(WebSecurity.CurrentUserId);
            bool lastResult = _userService.IsUserPermitedtoDelete(UID);
            if (lastResult == false)
            {

                return View("NoPermissionUser");
            }
            else
            {
                ViewBag.logUser = WebSecurity.CurrentUserId;
                UserInsert obj = new UserInsert();
                obj.RolesList = _userService.GetAllRoles();
                return View(obj);
            }
        }

        // GET: /Users/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Core.Domain.Users user = _userService.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /Users/Users/Create
        public ActionResult Create()
        {

            ViewBag.AllRoles = _userService.GetAllRoles();
            string IsSuperAdmin = "False";
            try
            {
                if (Session["TokenID"].ToString() != null && Session["TokenID"].ToString() != "")
                {
                    IsSuperAdmin = _userService.GetIsSuperAdmin(Session["TokenID"].ToString());
                }
                else
                {
                    return RedirectToAction("LogOut", "Account", new { area = "" });
                }
            }
            catch (Exception ex) { return RedirectToAction("LogOut", "Account", new { area = "" }); }
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            Core.Domain.UserInsert obj = new UserInsert();
            return View(obj);
        }

        // POST: /Users/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleException]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,Email,RoleIDs,Password,ConfirmPassword,IsSuperAdmin")] UserInsert user)
        {

            //if (user.RoleIDs == null)
            //{
            //    return Content("Please Select Role.");
            //}
            user.RolesList = _userService.GetAllRoles();
            ViewData["AllRole"] = _userService.GetAllRoles();
            if (ModelState.IsValid)
            {
                user.Active = true;
                Core.Domain.Users _usersObject = null;
                try
                {
                    _usersObject = _usersAPIController.Create(user);
                    WebSecurity.CreateAccount(_usersObject.Email, _usersObject.Password);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.logUser = WebSecurity.CurrentUserId;
                    UserInsert obj = new UserInsert();
                    obj.RolesList = _userService.GetAllRoles();
                  
                    List<Core.Domain.Users> list = _userService.GetUsersByEmail(user.Email.ToString());
                    if (list.Count > 0)
                    {
                        //_userService.DeleteUser(_usersObject.UserId);
                        return Content("This email is already registered");
                    }
                    return View("Index", obj);
                }


            }
            else
            {
                var avc = ModelState.Values.Select(x=>x.Errors);
            }
            return View("Index", user);
            // return null;
        }

        // GET: /Users/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Core.Domain.Users user = _usersAPIController.GetUserByID(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /Users/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Email,Password,ConfirmPassword,ModifiedBy")] Core.Domain.Users user)
        {

            if (ModelState.IsValid)
            {
                user.ModifiedBy = WebSecurity.CurrentUserId;
                _usersAPIController.EditUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /Users/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Core.Domain.Users user = _usersAPIController.GetUserByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /Users/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _usersAPIController.DeleteUser(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetUsers()
        {
            var data = from c in _userService.GetAllUsers()
                       select new
                       {
                           value = c.UserId,
                           text = c.FirstName + " " + c.LastName

                       };
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        //protected override void Dispose(bool disposing)
        //{

        //    base.Dispose(disposing);
        //}

        [HttpGet, ActionName("GetUserRolesById")]
        public JsonResult GetUserRolesById(int UserId)
        {
            var data = from c in _userService.GetUserRolesById(UserId)
                       select new
                       {
                           c.UserId,
                           c.RoleId

                       };
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost, ActionName("DeleteUserRolesById")]
        public JsonResult DeleteUserRolesById(int UserId, string Ids)
        {
            var data = _userService.DeleteUserRolesById(UserId, Ids);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("CreateUserRoles")]
        public JsonResult CreateUserRoles(int UserId, string Ids)
        {
            var data = "";
            _userService.CreateUserRoles(UserId, Ids);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("MassUpdateUser")]
        public JsonResult MassUpdateUser(string UserNames, string UnCheckIds, int ModifyBy)
        {
            var v = "";
            _userService.MassUpdateUser(UserNames, UnCheckIds, ModifyBy);
            return Json(v, JsonRequestBehavior.AllowGet);

        }

        //Show Edit Popup
        [HttpPost]
        [ActionName("ShowEditPopup")]
        public JsonResult ShowEditPopup(int UserId)
        {

            var v = _userService.GetUsersByUserID(UserId);
            return Json(v, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public bool IsUserExists(string Email)
        {
            bool IsExsits = false;
            List<Core.Domain.Users> list = _userService.GetUsersByEmail(Email);
            if (list.Count > 0)
            {
                IsExsits = true;
                ModelState.AddModelError("", "User already Exits.");
            }
            return IsExsits;
        }
        //SumitS
        [HttpPost]
        public JsonResult IsUserPermitedtoDelete()
        {
            try
            {                
                //var TokenID = Session["TokenID"].ToString();                
                long UID = Convert.ToInt64( WebSecurity.CurrentUserId);
                bool lastResult=_userService.IsUserPermitedtoDelete(UID);   
                return Json(lastResult,JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        //SumitS
        public ActionResult NoPermissionUser()
        {
            ViewBag.Message = "You are not authorized to view this page.";

            return View();
        }
    }
        #endregion
}
