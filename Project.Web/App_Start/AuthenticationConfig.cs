using System;
using System.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using Project.Web;

[assembly: OwinStartup(typeof(AuthenticationConfig))]

namespace Project.Web
{
    public class AuthenticationConfig
    {
        public void Configuration(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            var secretKey = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["Jwt:Key"]);
            var key = Encoding.UTF8.GetBytes(secretKey);

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }
            });
        }
    }
}