using System;
using System.Web.Mvc;

namespace EmployeeManagement.Common
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception currentException = filterContext.Exception;
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string actionMethod = filterContext.RouteData.Values["action"].ToString();

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error"
            };
        }
    }
}