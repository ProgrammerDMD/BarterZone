using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project.BusinessLogic.Core.JWT
{
    public class JwtAuthorizer
    {
        public static bool Authorize(HttpContextBase httpContext)
        {
            var authToken = HttpUtility.UrlDecode(httpContext.Request.Cookies["AuthToken"]?.Value);
            if (string.IsNullOrWhiteSpace(authToken)) return false;

            var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            var secretKey = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["Jwt:Key"]);
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(authToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                httpContext.User = principal;
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
        
        public static void DeleteAuthCookie(HttpContextBase httpContext)
        {
            var requestCookie = httpContext.Request.Cookies["AuthToken"];
            string cookiePath = "/";
            
            if (requestCookie != null)
            {
                if (!string.IsNullOrEmpty(requestCookie.Path))
                {
                    cookiePath = requestCookie.Path;
                }

                var expiredCookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    HttpOnly = true,
                    Path = cookiePath,
                    Secure = httpContext.Request.IsSecureConnection,
                    Value = string.Empty
                };
                httpContext.Response.Cookies.Add(expiredCookie);
            }
            else
            {
                var expiredCookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    HttpOnly = true,
                    Path = "/",
                    Secure = httpContext.Request.IsSecureConnection,
                    Value = string.Empty
                };
                httpContext.Response.Cookies.Set(expiredCookie);
            }
        }
    }
}
