using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Interface
{
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// 收費服務
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        Task<string> CallCpApi(string apiName, T instance);

        /// <summary>
        /// 免費服務
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        Task<string> CallOpApi(string apiName, T instance);
    }
}
