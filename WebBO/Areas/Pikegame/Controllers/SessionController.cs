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

		#region 取得比賽場次列表
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
					SELECT      sessionid,
										tournamentid,
										accountid,
										sessionname,
										sessiontime::TEXT,
										red_accountid,
										blue_accountid,
										redfraction_sum,
										bluefraction_sum,
										mstatus,
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
											) blue_account,
											is_equipment_exist(sessionid)
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
        #endregion

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


		#region 新增比賽場次列表
		/// <summary>
		/// 新增比賽場次列表
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult SetSession(SessionModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
            #region  sql
            querySql.Append(@"
				
			INSERT INTO PUBLIC.session (
								tournamentid,
								accountid,
								sessionname,
								sessiontime,
								red_accountid,
								blue_accountid,							
								mstatus
							)
						VALUES (
					@tournamentid,
					@judge_accountid,
					@sessionname,
					@sessiontime,
					@red_accountid,
					@blue_accountid,				
					@mstatus
						);
			       ");
			#endregion

			parm.Add("@tournamentid", request.tournamentid);
			parm.Add("@judge_accountid", request.judge_accountid);
			parm.Add("@sessionname", request.sessionname);
			parm.Add("@sessiontime", request.sessiontime);
			parm.Add("@red_accountid", request.red_accountid);
			parm.Add("@blue_accountid", request.blue_accountid);			
			parm.Add("@mstatus", 0);

			var dt = new DataTable();		

			//dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}

        #endregion

    }
}
