using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBO.Areas.Pikegame.Models;
using WebBO.General;
using WebBO.General.Repository.Connection;

namespace WebBO.Areas.Pikegame.Controllers
{
    public class SessionController
    {
        protected ConnectionFactory _connectionFactory;
        public SessionController()
        {
            _connectionFactory = new ConnectionFactory();
        }

		/// <summary>
		/// 取得比賽場次列表
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetSessionForWeb()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			         
						SELECT s.*,
						(
							SELECT accountname
							FROM accountm AS a
							WHERE a.accountid = s.accountid
							) judge_account,
						(
							SELECT accountname
							FROM accountm AS a
							WHERE a.accountid = s.red_accountid
							) red_account,
						(
							SELECT accountname
							FROM accountm AS a
							WHERE a.accountid = s.blue_accountid
							) blue_account
					FROM PUBLIC.session s
					ORDER BY sessionid ASC

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

		/// <summary>
		/// 取得比賽場次列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetSession()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			         
						SELECT s.*,
						(
							SELECT accountname
							FROM accountm AS a
							WHERE a.accountid = s.accountid
							) judge_account,
						(
							SELECT accountname
							FROM accountm AS a
							WHERE a.accountid = s.red_accountid
							) red_account,
						(
							SELECT accountname
							FROM accountm AS a
							WHERE a.accountid = s.blue_accountid
							) blue_account
					FROM PUBLIC.session s
					ORDER BY sessionid ASC

            ");

			var dt = new DataTable();
			dt.Load(cn.ExecuteReader(querySql.ToString()));

			return dt;			
		}

	}
}
