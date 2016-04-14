using Core.Domain;
using DTOLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceLayer.Email;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using smartData.Messages;
using smartData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Security;
using WebMatrix.WebData;

namespace smartData.Controllers
{
    public class AccountAPIController : ApiController
    {
        IEmailService _emailService;
        IUserService _userService;
        IScreenPermissionService _screenPermissionService;
        public AccountAPIController(IScreenPermissionService screenPermissionService, IEmailService emailService, IUserService userService)
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())),
            new EmailService(new Config(), new HtmlMessageFormatter(new Config())
                , new EmailMessageSender(new Config())), screenPermissionService, userService)
        {
        }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public AccountAPIController(UserManager<ApplicationUser> userManager, IEmailService emailService, IScreenPermissionService screenPermissionService, IUserService userService)
        {
            _emailService = emailService;
            _userService = userService;
            UserManager = userManager;
            _screenPermissionService = screenPermissionService;
        }

        [AllowAnonymous]
        [HttpPost]
        public Users Login(LoginDTO model)
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

                        return _userService.GetUserById(cID);
                    }
                    else
                    {
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
                    }
                }
                else
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
                }

            }
            return new Users();
        }

        public string Register(UserRegistrationDTO model)
        {
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
                    //TODO This Code Use For Mainain Password History
                    string RetPassword = HashData(model.Password);
                    SecUserPasswordHistory _secUserPasswordHistory = new SecUserPasswordHistory();
                    byte[] array = Encoding.ASCII.GetBytes(RetPassword);

                    _secUserPasswordHistory.PasswordHash256 = array;
                    _secUserPasswordHistory.DeleteFlag = false;
                    _secUserPasswordHistory.RowVersion = null;
                    _secUserPasswordHistory.SecUserID = WebSecurity.GetUserId(model.UserName);
                    _userService.AddPasswordHistory(_secUserPasswordHistory);
                    

                    ModelState.AddModelError("", CustomMessages.UserRegSuccess);

                    _emailService.SendRegistrationEmail(model.UserName, model.UserName, model.UserName);

                    return CustomMessages.UserRegSuccess;
                }
                catch (Exception ex)
                {
                    return "User already exit..";
                }
            }
            //ModelState.AddModelError("", "User already exist..");
            // If we got this far, something failed, redisplay form
            return "";
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


    }
}
