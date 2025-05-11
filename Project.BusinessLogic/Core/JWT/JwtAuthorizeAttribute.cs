using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.BusinessLogic.Core.JWT;

namespace Project.BusinessLogic.Core
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public JwtAuthorizeAttribute(bool redirectToLogin = true)
        {
            RedirectToLogin = redirectToLogin;
        }

        public JwtAuthorizeAttribute(string role, bool redirectToLogin = true) : this(redirectToLogin)
        {
            if (!string.IsNullOrWhiteSpace(role)) Role = role;
        }

        public string Role { get; set; }
        public bool RedirectToLogin { get; set; }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return JwtAuthorizer.Authorize(httpContext, Role);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (RedirectToLogin)
            {
                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
            else
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                else
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }
    }
}