using isRock.Framework.WebAPI;
using WebBO.General.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;


namespace WebAPI.Areas.BusStopManagement.Controllers
{
    public class BusStopController : ApiController
    {        
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("BusStopManagement/BusStop/{MethodName}")]
        [HttpPost]
        public IHttpActionResult ExecuteMethod(string MethodName)
        {
            try
            {
                //AssemblyLauncher
                AssemblyLauncher assemblyLauncher = new AssemblyLauncher();
                //執行指定的Method                
                var ret = assemblyLauncher.ExecuteCommand(
                    new WebBO.Areas.BusStopManagement.Controllers.BusStopController(),
                    MethodName,
                   Request.Content.ReadAsStringAsync().Result
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