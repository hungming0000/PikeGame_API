using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;


namespace WebAPI.Class
{


    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");

            base.OnActionExecuting(filterContext);
        }
    }
    public class RawJsonActionResult : IHttpActionResult
    {
        private readonly string _jsonString;

        public RawJsonActionResult(string jsonString)
        {
            _jsonString = jsonString;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var content = new StringContent(_jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
            var contentType = response.Content.Headers.ContentType;

            // Set response charset if not exist
            if (string.IsNullOrEmpty(contentType.CharSet))
            { contentType.CharSet = "utf-8"; }

            return Task.FromResult(response);
        }
    }
}