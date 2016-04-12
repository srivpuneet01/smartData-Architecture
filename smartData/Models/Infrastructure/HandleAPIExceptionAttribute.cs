using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace smartData.Infrastructure
{
    public class HandleAPIExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);

            }

            Exception ex = (Exception)context.Exception;


            string CustomExceptionType = String.Empty;
            CustomExceptionType = ex.GetType().Name;
            if (CustomExceptionType == "CustomMessage")

            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = "CustomMessage"
                });
            }
        }
    }
}