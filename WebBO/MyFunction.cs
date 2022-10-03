using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects.DataClasses;

namespace WebBO
{
    /// <summary>
    /// 在 EF 中使用自定義的 SQL Function
    /// EF 6 版，要用 DbFunction
    /// EF 5 版，要用 EdmFunction
    /// </summary>
    public partial class NT_PlantTreeEntities
    {
        /// <summary>
        /// 呼叫SQL的TM_GetResurveyCountByLand，回傳該地號是否有舊地號
        /// </summary>
        /// <param name="CityName"></param>
        /// <param name="TownName"></param>
        /// <param name="MainSec"></param>
        /// <param name="SubSec"></param>
        /// <param name="LandNo"></param>
        /// <returns></returns>
        [DbFunction("NT_PlantTreeModel.Store", "TM_GetResurveyCountByLand")]
        public int? TM_GetResurveyCountByLand(string CityName, string TownName, string MainSec, string SubSec, string LandNo)
        {
            //不需實做，直接會呼叫sql的函數
            throw new NotSupportedException("Direct calls are not supported.");
        }
    }
}
