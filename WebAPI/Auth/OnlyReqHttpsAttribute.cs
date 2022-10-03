using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAPI.Providers
{
    public class OnlyReqHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required",
                    Content = new StringContent("HTTPS Required", System.Text.Encoding.UTF8, "application/json"),
                };
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}