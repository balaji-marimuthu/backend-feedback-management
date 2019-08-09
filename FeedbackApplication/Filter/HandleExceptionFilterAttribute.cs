using FeedbackApplication.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace FeedbackApplication.Filter
{
    [ExcludeFromCodeCoverage]
    public class HandleExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {

            LogService.Logger().Error("Error Occurred: ", context.Exception);

            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}