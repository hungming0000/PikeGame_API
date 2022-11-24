using isRock.Framework.WebAPI;
using WebBO.General.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Specialized;

namespace WebAPI.Areas.Pikegame.Controllers
{
    public class WinFormController : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[Route("Pikegame/SessionSelect/GetSessionSelect")]
        //[HttpGet]
        //public IHttpActionResult Get(int id)
        //{

        //    var select = new WebBO.Areas.Pikegame.Controllers.WinFormController().GetSessionSelectData(id);
        //    return Ok(select);  // Returns an OkNegotiatedContentResult
        //}

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("Pikegame/WinForm/{MethodName}")]
        [HttpGet]
        public IHttpActionResult ExecuteMethod(string MethodName)
        {
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                var tid =  nvc["tid"];


                //AssemblyLauncher
                AssemblyLauncher assemblyLauncher = new AssemblyLauncher();
                //執行指定的Method                
                var ret = assemblyLauncher.ExecuteCommand(
                    new WebBO.Areas.Pikegame.Controllers.WinFormController(),
                    MethodName,
                    tid
                    // Request.Content.ReadAsStringAsync().Result
                    );
                //回傳OK
                return Ok(ret);
            }
            catch (Exception ex)
            {
                //其他處理
                throw ex;
            }
        }
    }
}