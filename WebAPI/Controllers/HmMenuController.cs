using isRock.Framework.WebAPI;
using WebBO.General.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;


namespace WebAPI.Controllers
{

    public class HmMenuController : ApiController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MethodName">要執行的方法</param>
        /// <returns></returns>
        ///         
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("HmMenu/{MethodName}")]
        [HttpPost]
        public IHttpActionResult ExecuteMethod(string MethodName)
        {
            try
            {
                //AssemblyLauncher
                AssemblyLauncher assemblyLauncher = new AssemblyLauncher();
                //執行指定的Method                
                var ret = assemblyLauncher.ExecuteCommand(
                    new WebBO.Controllers.HmMenuController(),
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