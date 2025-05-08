using Microsoft.IdentityModel.Tokens;
using Project.BusinessLogic.Core.JWT;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Project.BusinessLogic.Core
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public string Role { get; set; }
        public bool RedirectToLogin { get; set; }

        public JwtAuthorizeAttribute(bool redirectToLogin = true)
        {
            RedirectToLogin = redirectToLogin;
        }
        
        public JwtAuthorizeAttribute(string role, bool redirectToLogin = true) : this(redirectToLogin)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                Role = role;
            }
        }


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
                {
                    filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                }
                else
                {
                    filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
