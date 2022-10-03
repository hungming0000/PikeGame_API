using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Repository.Connection;
using WebAPI.App_Start;
using WebAPI.Providers;

[assembly: OwinStartup(typeof(WebAPI.Startup))]

namespace WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            ConfigureOAuth(app);
            app.UseWebApi(config);
        }


        public class DashboardBasicAuthorizationFilter : IDashboardAuthorizationFilter
        {
            private bool Challenge(OwinContext context)
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
                context.Response.Write("Authenticatoin is required.");
                return false;
            }

            public bool Authorize(DashboardContext context)
            {


                return true;
                // In case you need an OWIN context, use the next line, `OwinContext` class
                // is the part of the `Microsoft.Owin` package.

                var owinContext = new OwinContext(context.GetOwinEnvironment());


                //公司外網使用時忽略條款
                /*
                if (owinContext.Request.Scheme != "https")
                {
                    string redirectUri = new UriBuilder("https", owinContext.Request.Host.ToString(), 443, context.Request.Path).ToString();

                    owinContext.Response.StatusCode = 301;
                    owinContext.Response.Redirect(redirectUri);
                    return false;
                }
                if (owinContext.Request.IsSecure == false)
                {
                    owinContext.Response.Write("Secure connection is required to access Hangfire Dashboard.");
                    return false;
                }
                */


                var user = owinContext.Authentication.User;
                if (user != null)
                {
                    if (user.Identity.IsAuthenticated)
                    {
                        return true;
                    }
                }

                // Allow all authenticated users to see the Dashboard (potentially dangerous).
                string header = owinContext.Request.Headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(header))
                {
                    var auHeader = AuthenticationHeaderValue.Parse(header);
                    if ("Basic".Equals(auHeader.Scheme, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var split = Encoding.UTF8
                                            .GetString(Convert.FromBase64String(auHeader.Parameter))
                                            .Split(':');
                        if (split.Length == 2)
                        {
                            string userId = split[0];
                            string password = split[1];
                            if (string.Compare(userId, "ivis", true) == 0 &&
                                string.Compare(password, "abcd0803", true) == 0)
                            {
                                var claims = new List<Claim>();
                                claims.Add(new Claim(ClaimTypes.Name, "yao"));
                                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                                var identity = new ClaimsIdentity(claims, "HangfireLogin");
                                owinContext.Authentication.SignIn(identity);
                                return true;
                            }
                        }
                    }
                }

                return this.Challenge(owinContext);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //允許客戶端使用http協議請求,正式時取消
                #if DEBUG
                AllowInsecureHttp = true,
                #endif
                //token請求的地址，即 http://localhost:prot/token
                TokenEndpointPath = new PathString("/token"),
                //Token過期時間-這裡設單位為分鐘，失驗證就要重登，至少要重抓auth
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                //提供具體的認證方式
                Provider = new ADAuthorizationServerProvider(),
                ApplicationCanDisplayErrors = true,
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
