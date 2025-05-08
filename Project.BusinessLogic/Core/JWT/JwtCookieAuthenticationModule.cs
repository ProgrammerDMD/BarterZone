using Microsoft.IdentityModel.Tokens;
using Project.BusinessLogic.Core.JWT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project.BusinessLogic.Core
{
    public class JwtCookieAuthenticationModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext httpContext = app.Context;

            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var httpContextBase = new HttpContextWrapper(httpContext);
            JwtAuthorizer.Authorize(httpContextBase);
        }

        public void Dispose() { }
    }
}
