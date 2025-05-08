using Microsoft.IdentityModel.Tokens;
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
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authToken = HttpUtility.UrlDecode(httpContext.Request.Cookies["AuthToken"]?.Value);
            if (string.IsNullOrWhiteSpace(authToken)) return false;

            var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            var secretKey = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["Jwt:Key"]);
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(authToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;
                var identity = new ClaimsIdentity(jwtToken.Claims, "JWT");
                httpContext.User = new ClaimsPrincipal(identity);

                return true;
            }
            catch (SecurityTokenValidationException stve)
            {
                Debug.WriteLine($"JwtAuthorizeAttribute: SecurityTokenValidationException: {stve.Message}");
                if (stve.InnerException != null)
                {
                    Debug.WriteLine($"JwtAuthorizeAttribute: Inner Exception: {stve.InnerException.Message}");
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"JwtAuthorizeAttribute: General Exception during token validation: {e.ToString()}");
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Auth/Login");
        }
    }
}
