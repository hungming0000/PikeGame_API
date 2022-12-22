using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBO.General;
using WebBO.General.Repository.Connection;

namespace WebBO.Areas.Pikegame.Controllers
{
   public  class AdvertisesettingController
    {
        protected ConnectionFactory _connectionFactory;
        public AdvertisesettingController()
        {
            _connectionFactory = new ConnectionFactory();
        }

		/// <summary>
		/// 取得比賽場次列表
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetAdvertisesetting()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;

			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			         
					SELECT *
					FROM PUBLIC.advertisesetting
					WHERE is_timeperiod_exist(advertiseid, periodtimes) = true
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
