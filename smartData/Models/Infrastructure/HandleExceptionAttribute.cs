using smartData.Controllers;
using Core.enums;
using Core.Exceptions;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using smartData.Messages;

namespace smartData.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property,  AllowMultiple = false)]
    public class HandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            RouteData currentRouteData = filterContext.RouteData.Route.GetRouteData(filterContext.HttpContext);
            String currentController = String.Empty, currentAction = String.Empty;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //Because its a exception raised after ajax invocation
                //Lets return Json
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                return;
            }
            
            if (currentRouteData != null)
            {
                currentController = currentRouteData.Values["controller"].ToString();
                currentAction = currentRouteData.Values["action"].ToString();
            }
            filterContext.ExceptionHandled = true;
            //filterContext.HttpContext.Response.StatusCode = 200;
            //filterContext.HttpContext.ClearError();

            filterContext.Result = new ViewResult()
            {
                ViewName = currentAction,
                
            };
            return;

            base.OnException(filterContext);
            
            Exception ex = (Exception)filterContext.Exception;
            
            HttpContextBase httpContext = filterContext.HttpContext;
            httpContext.ClearError();
           
            RouteData routeData = new RouteData();
            string CustomExceptionType = String.Empty;
            CustomExceptionType = ex.GetType().Name;
            if (CustomExceptionType == "CustomMessage")
            {
                CustomMessage cm = (CustomMessage)filterContext.Exception;
                routeData.Values["ExceptionMessage"] = cm.Message;
                var viewData = new ViewDataDictionary<HandleErrorInfo>(filterContext.Controller.ViewData);
                viewData.Add("ExceptionMessage",cm.Message);
                httpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;
              
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
            else
            {

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (Exception)ex is HttpException ? ((HttpException)(Exception)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

           
            
            Error404Controller controller = new Error404Controller();

            routeData.Values["controller"] = CustomMessages.Error404;



            routeData.Values["action"] = CustomMessages._ErrorView;
            routeData.Values["ExceptionMessage"] = CustomMessages.ErrorPlzTryAgain;
            routeData.Values["IsAjaxRequest"] = filterContext.HttpContext.Request.IsAjaxRequest();
           

                if (CustomExceptionType == Convert.ToString(ExpeptionTypes.LoginException))
                {
                    routeData.Values["ExceptionType"] = (Int32)ExceptionTypeEnum.LogoutWithAjaxRequest;
                }
                else
                {
                    routeData.Values["ExceptionType"] = (Int32)ExceptionTypeEnum.Exception;
                    if (CustomExceptionType == Convert.ToString(ExpeptionTypes.CustomMessage))
                    {
                        CustomMessage cm = (CustomMessage)filterContext.Exception;
                        routeData.Values["ExceptionMessage"] = cm.Message;
                    }
                    else
                    {
                        if (CustomExceptionType == Convert.ToString(ExpeptionTypes.CustomException))
                        {
                            CustomException ce = (CustomException)filterContext.Exception;
                            routeData.Values["ExceptionMessage"] = ce.Message;
                            //ex = ce.Exception;
                        }
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }
                controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
                ((IController)controller).Execute(new RequestContext(filterContext.HttpContext, routeData));
            }
        }
    }   
}