using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using WebBO.Controllers;

namespace WebAPI.Providers
{
    public class ADAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public ADAuthorizationServerProvider()
        {

        }
        /// <summary>
        /// 用來對third party application 認證，具體的做法是為third party application頒發appKey和appSecrect
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (context.TryGetFormCredentials(out clientId, out clientSecret))
            {

                context.Validated();
            }
            return Task.FromResult(0);
        }
        /// <summary>
        /// 這裡驗證用戶名和密碼是否正確，採用了ClaimsIdentity認證方式，context.Validated(ticket); 表明認證通過
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {            
            UserInfo user = null;
            using (ADAuth _repo = new ADAuth())
            {
                try
                {
                    user = await _repo.FindUser(context.UserName, context.Password);
                    if (!user.IsEnable)
                    {
                        string ErrMsg = "使用者帳號或是密碼不正確!";
                        context.SetError("invalid_grant", ErrMsg);//webexception json s
                        return;
                    }

                }
                catch (Exception Ex)
                {
                    string ErrMsg = "驗證帳號時，發生下列錯誤：" + Ex.Message;
                    context.SetError("invalid_grant", ErrMsg);
                    return;
                }
            }
            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));
            AuthenticationProperties props = new AuthenticationProperties(new Dictionary<string, string>{
             {"as:client_id" , context.ClientId ?? string .Empty },
             {"userName" , context.UserName },
             {"userInfo",JsonConvert.SerializeObject(user)}});
            AuthenticationTicket ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }
        /// <summary>
        /// 這裡實作使用 client_credentials 的 grant_type
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(context.ClientId, OAuthDefaults.AuthenticationType), context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
            context.Validated(identity);
            return Task.FromResult(0);
        }
        /// <summary>
        /// 將會把Context中的屬性加入到token中，只有 ValidateClientAuthentication & GrantResourceOwnerCredentials都過了，才會給token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}