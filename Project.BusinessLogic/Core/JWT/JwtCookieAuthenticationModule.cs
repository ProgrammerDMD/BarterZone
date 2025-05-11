using System;
using System.Web;
using Project.BusinessLogic.Core.JWT;

namespace Project.BusinessLogic.Core
{
    public class JwtCookieAuthenticationModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        public void Dispose()
        {
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var httpContext = app.Context;

            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated) return;

            var httpContextBase = new HttpContextWrapper(httpContext);
            JwtAuthorizer.Authorize(httpContextBase);
        }
    }
}