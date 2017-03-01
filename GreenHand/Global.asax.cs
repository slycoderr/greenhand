using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Routing;

namespace GreenHand
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new ExceptionFilterAttribute());
        }
    }

    public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is SecurityException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            else if (context.Exception is ArgumentException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                context.Response.Content = new StringContent(context.Exception.Message);
            }

            else
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
