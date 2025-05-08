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
        public bool RedirectToLogin { get; set; }

        public JwtAuthorizeAttribute(bool redirectToLogin = true)
        {
            RedirectToLogin = redirectToLogin;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return JwtAuthorizer.Authorize(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (RedirectToLogin)
            {
                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
