using System.Web.Mvc;
using System.Web.Routing;

namespace HRM.Core.Helper
{
    public class Authenticate : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (AuthBase.UserId <= 0)
            {
                RouteValueDictionary routeValueDictionaries = new RouteValueDictionary()
                {
                    { "action", "Index" },
                    { "controller", "Login" },
                    { "returnUrl", filterContext.HttpContext.Request.RawUrl }
                };
                filterContext.Result = new RedirectToRouteResult(routeValueDictionaries);
            }
        }
    }
}