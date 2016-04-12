using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using smartData.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using smartData.Common;
using Core.Domain;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebMatrix.WebData;
using System.Security.Cryptography;
using System.Text;
using ServiceLayer.Services;
using smartData.Messages;
using ServiceLayer.Interfaces;
using ServiceLayer.Email;
using MvcBasicSite.Models;

namespace smartData.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IEmailService _emailService;
        IUserService _userService;
        IScreenPermissionService _screenPermissionService;
        public AccountController(IScreenPermissionService screenPermissionService, IEmailService emailService, IUserService userService)
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())),
            new EmailService(new Config(), new HtmlMessageFormatter(new Config())
                , new EmailMessageSender(new Config())), screenPermissionService, userService)
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager, IEmailService emailService, IScreenPermissionService screenPermissionService, IUserService userService)
        {
            _emailService = emailService;
            _userService = userService;
            UserManager = userManager;
            _screenPermissionService = screenPermissionService;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
            }
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel eTracLogin = new LoginViewModel();
            if (Request.Cookies["LoginCookie"] != null)
            {
                try
                {
                    eTracLogin.UserName = Request.Cookies["LoginCookie"]["UserName"];
                    eTracLogin.Password = Request.Cookies["LoginCookie"]["pwd"]; //Cryptography.GetDecryptedData(Request.Cookies["LoginCookie"]["pwd"], true); ;
                    eTracLogin.RememberMe = true;
                    return View(eTracLogin);
                }
                catch
                {
                    return View();
                }
            }
            else
                return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            // ServiceLayer.Services.ScreenPermissionService _ActionAccessPermissionService = new ServiceLayer.Services.ScreenPermissionService();

            //// If we got this far, something failed, redisplay form
            //return View(model);
            if (ModelState.IsValid)
            {
                // ServiceLayer.Services.ResetPasswordService _ResetPasswordService = new ServiceLayer.Services.ResetPasswordService();
                List<Users> list = _userService.GetUsersByEmail(model.UserName.ToString());
                if (list.Count > 0)
                {
                    if (WebSecurity.Login(model.UserName, model.Password))
                    {
                        int cID = WebSecurity.GetUserId(model.UserName);
                        string TokenID = _screenPermissionService.GetAuthorizeToken(Convert.ToInt32(cID));
                        Session["TokenID"] = TokenID;
                        if (Session["TokenID"].ToString() == "")
                        {
                            TokenID = _screenPermissionService.GetAuthorizeToken(Convert.ToInt32(cID));
                            Session["TokenID"] = TokenID;
                        }
                        if (model.RememberMe)
                        {
                            CreateAuthenticateFormsTicket(model);
                        }
                        if (returnUrl != null && returnUrl != "/")
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", CustomMessages.InvalidUserOrPass);
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", CustomMessages.InvalidUserOrPass);
                    return View(model);
                }

            }
            ModelState.AddModelError("", CustomMessages.InvalidUserOrPass);
            return View(model);
        }

        /// <summary>
        /// Change the current culture.
        /// </summary>
        /// <param name="culture">The current selected culture.</param>
        /// <returns>The action result.</returns>
        [AllowAnonymous]
        public ActionResult ChangeCurrentCulture(int culture)
        {
            //
            // Change the current culture for this user.
            //
            SiteSession.CurrentUICulture = culture;
            //
            // Cache the new current culture into the user HTTP session. 
            //
            Session["CurrentUICulture"] = culture;
            //
            // Redirect to the same page from where the request was made! 
            //
            return Redirect(Request.UrlReferrer.ToString());
        }


        [NonAction]
        private void CreateAuthenticateFormsTicket(LoginViewModel eTracLogin)
        {
            try
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             eTracLogin.UserName.ToString(),                                 //user Name
                             DateTime.Now,
                             DateTime.Now.AddMinutes(30),                          // expiry in 30 min
                             eTracLogin.RememberMe, "");

                if (eTracLogin.RememberMe)
                {
                    string formsCookieStr = string.Empty;

                    formsCookieStr = FormsAuthentication.Encrypt(authTicket);

                    HttpCookie FormsCookie = new HttpCookie("LoginCookie", formsCookieStr);
                    FormsCookie.Expires = DateTime.Now.AddDays(15);

                    FormsCookie["UserName"] = eTracLogin.UserName;
                    FormsCookie["pwd"] = eTracLogin.Password;

                    HttpContext.Response.Cookies.Add(FormsCookie);

                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie myCookie = new HttpCookie("LoginCookie");
                    myCookie.Expires = DateTime.Now;
                    Response.Cookies.Add(myCookie);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                }
                Session["eTrac"] = eTracLogin;
            }
            catch (Exception ex)
            { throw ex; }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "Account");
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            //ServiceLayer.Services.ResetPasswordService _ResetPasswordService = new ServiceLayer.Services.ResetPasswordService();

            if (ModelState.IsValid)
            {

                try
                {
                    List<Users> list = _userService.GetUsersByEmail(model.UserName.ToString());
                    int _userID = WebSecurity.GetUserId(model.UserName);
                    if (list.Count == 0 && _userID > 0)
                    {
                        ((SimpleMembershipProvider)Membership.Provider).DeleteUser(model.UserName.ToString(), true); // deletes record from webpages_Membership table
                    }
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    //eTODO This Code Use For Mainain Password History
                    string RetPassword = HashData(model.Password);
                    SecUserPasswordHistory _secUserPasswordHistory = new SecUserPasswordHistory();
                    byte[] array = Encoding.ASCII.GetBytes(RetPassword);

                    _secUserPasswordHistory.PasswordHash256 = array;
                    _secUserPasswordHistory.DeleteFlag = false;
                    _secUserPasswordHistory.RowVersion = null;
                    _secUserPasswordHistory.SecUserID = WebSecurity.GetUserId(model.UserName);
                    _userService.AddPasswordHistory(_secUserPasswordHistory);
                    //End

                    ModelState.AddModelError("", CustomMessages.UserRegSuccess);

                    _emailService.SendRegistrationEmail(model.UserName, model.UserName, model.UserName);

                    //string from = "time2cutest@gmail.com";
                    //using (MailMessage mail = new MailMessage(from, model.UserName))
                    //{
                    //    mail.Subject = CustomMessages.RegNotification;
                    //    mail.IsBodyHtml = false;
                    //    SmtpClient smtp = new SmtpClient();
                    //    smtp.Host = "smtp.gmail.com";
                    //    smtp.EnableSsl = true;
                    //    NetworkCredential networkCredential = new NetworkCredential(from, "Tej@1234");
                    //    smtp.UseDefaultCredentials = true;
                    //    smtp.Credentials = networkCredential;
                    //    smtp.Port = 587;
                    //    smtp.Send(mail);
                    //}

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // ModelState.AddModelError("", "User already exist..");
                }
            }
            ModelState.AddModelError("", "User already exist..");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? CustomMessages.YourPassHasBeenChanged
                : message == ManageMessageId.SetPasswordSuccess ? CustomMessages.YourPassHasBeenSet
                : message == ManageMessageId.RemoveLoginSuccess ? CustomMessages.ExtLoginWasRemoved
                : message == ManageMessageId.Error ? CustomMessages.ErrorOccured
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {

            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            // var user = WebSecurity.GetUserId(User.Identity.GetUserName());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion



        #region Login Verification

        /// <summary>
        /// Create cookie for authentication
        /// </summary>
        /// <param name="model"></param>
        private void CreateAuthorizedCookiesFronEndUser(Users model)
        {

            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.UserId = model.UserId;
            serializeModel.FirstName = model.FirstName;
            serializeModel.Email = model.Email;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            //serialize user data and add to cookie
            string userData = serializer.Serialize(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                model.Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false, userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);

            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        /// <summary>
        /// LogOut User
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLogout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        #endregion

        #region ForgotPassword
        // [HttpPost]
        //   [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword()
        {


            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ManageUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OldPassword != model.NewPassword)
                {
                    //ServiceLayer.Services.ResetPasswordService _ResetPasswordService = new ServiceLayer.Services.ResetPasswordService();
                    var token = "";
                    string UserName = WebSecurity.CurrentUserName;
                    //check user existance


                    var user = Membership.GetUser(UserName);

                    bool changePasswordSucceeded;
                    changePasswordSucceeded = user.ChangePassword(model.OldPassword, model.NewPassword);

                    if (!changePasswordSucceeded)
                    {
                        return Content(CustomMessages.CurrentPassNotCorrect);
                    }

                    if (user == null)
                    {
                        TempData["Message"] = CustomMessages.UserNotExist;
                    }
                    else
                    {
                        //generate password token
                        token = WebSecurity.GeneratePasswordResetToken(UserName);
                        //create url with above token
                    }
                    bool any = _userService.UpdatePassword(UserName, token);
                    bool response = false;
                    if (any == true)
                    {
                        response = WebSecurity.ResetPassword(token, model.NewPassword);
                        if (response == true)
                        {
                            try
                            {
                                //  Here Maintain Password History
                                //  MembershipUser u = Membership.GetUser(WebSecurity.CurrentUserName, false);

                                string RetPassword = HashData(model.NewPassword);
                                SecUserPasswordHistory _secUserPasswordHistory = new SecUserPasswordHistory();
                                byte[] array = Encoding.ASCII.GetBytes(RetPassword);

                                _secUserPasswordHistory.PasswordHash256 = array;
                                _secUserPasswordHistory.DeleteFlag = false;
                                _secUserPasswordHistory.RowVersion = null;
                                _secUserPasswordHistory.SecUserID = (WebSecurity.CurrentUserId);
                                _userService.AddPasswordHistory(_secUserPasswordHistory);
                                //TempData["Message"] = CustomMessages.PasswordChanged;
                                return Content(CustomMessages.PasswordChanged);
                            }
                            catch (Exception ex)
                            {
                                TempData["Message"] = CustomMessages.ErrorWhileChangingPassword + ex.Message;
                            }
                        }
                        else
                        {
                            TempData["Message"] = CustomMessages.HeyAvoidRandomRequest;
                        }
                    }
                    else
                    {
                        TempData["Message"] = CustomMessages.UserAndTokenNotMatch;
                    }

                }
                else
                {
                    return Content(CustomMessages.PasswordsMustbeDiff);
                }

            }
            return View(model);
        }

        static string HashData(string data)
        {

            SHA256 hasher = SHA256Managed.Create();

            byte[] hashedData = hasher.ComputeHash(

                Encoding.Unicode.GetBytes(data));



            // Now we'll make it into a hexadecimal string for saving

            StringBuilder sb = new StringBuilder(hashedData.Length * 2);

            foreach (byte b in hashedData)
            {

                sb.AppendFormat("{0:x2}", b);

            }

            return sb.ToString();

        }

        #endregion
    }
}