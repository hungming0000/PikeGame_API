using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebAPI.Providers;
using WebAPI.Models;
using Microsoft.AspNet.SignalR;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebAPI.Startup))]
namespace WebAPI
{
    public partial class Startup
    {

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            ConfigureOAuth(app);
            app.UseWebApi(config);
        }


        // 如需設定驗證的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301864
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

        // 註銷下列各行以啟用利用協力廠商登入提供者登入
        //app.UseMicrosoftAccountAuthentication(
        //    clientId: "",
        //    clientSecret: "");

        //app.UseTwitterAuthentication(
        //    consumerKey: "",
        //    consumerSecret: "");

        //app.UseFacebookAuthentication(
        //    appId: "",
        //    appSecret: "");

        //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
        //{
        //    ClientId = "",
        //    ClientSecret = ""
        //});

    }
}
