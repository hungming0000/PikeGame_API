using isRock.Framework.WebAPI;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPI.Areas.Pikegame.Controllers
{
    public class EquipmentsettingController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("Pikegame/Equipmentsetting/{MethodName}")]
        [HttpPost]
        public IHttpActionResult ExecuteMethod(string MethodName)
        {
            try
            {
                //AssemblyLauncher
                AssemblyLauncher assemblyLauncher = new AssemblyLauncher();
                //執行指定的Method                
                var ret = assemblyLauncher.ExecuteCommand(
                    new WebBO.Areas.Pikegame.Controllers.EquipmentsettingController(),
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