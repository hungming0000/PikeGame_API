using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBO.General;
using WebBO.General.Repository.Connection;

namespace WebBO.Areas.BusStopManagement.Controllers
{
    public class IntelStopController
    {
        protected ConnectionFactory _connectionFactory;
        public IntelStopController()
        {
            _connectionFactory = new ConnectionFactory();

        }
		/// <summary>
		/// 取得智慧站牌
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetVebusintelstopattr()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
			                 SELECT imsi,
											vtid,
											alivetime,
											code,
											connect_time,
											disconnect_time,
											mname,
											name,
											CONCAT (
												parameter,
												'秒'
												) parameter,
											regioncode,
											STATE as busstate,
											stoptypedesc,
											sys_type,
											routes,
											positions
										FROM operationpolicy.vebusintelstopattr t1

            ");

			var dt = new DataTable();
			dt.Load(cn.ExecuteReader(querySql.ToString()));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}


	}
}
