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
										mstatus::text,
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
								redfraction_sum,
								bluefraction_sum,
								mstatus
							)
						VALUES (
					@tournamentid,
					@judge_accountid,
					@sessionname,
					@sessiontime,
					@red_accountid,
					@blue_accountid,		
					@redfraction_sum,
					@bluefraction_sum,
					@mstatus
						);
			       ");
			#endregion

			parm.Add("@tournamentid", request.tournamentid);
			parm.Add("@judge_accountid", request.judge_accountid);
			parm.Add("@sessionname", request.sessionname);
			parm.Add("@sessiontime", Convert.ToDateTime(request.sessiontime));
			parm.Add("@red_accountid", request.red_accountid);
			parm.Add("@blue_accountid", request.blue_accountid);
			parm.Add("@redfraction_sum", 0);
			parm.Add("@bluefraction_sum", 0);
			parm.Add("@mstatus", 0);

			var dt = new DataTable();		

			dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}

		#endregion

		#region 開始/結束 比賽
		public ExecuteCommandAPIResult SetSessionmstatus(SessionModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			var selectparm = new DynamicParameters();
			var dt = new DataTable();
			var selectdt = new DataTable();
			bool is_equipment_exist;

			//查詢該裁判是否有正在比賽中的場次
			querySql.Append(@"	
				SELECT *
				FROM PUBLIC.session
				WHERE accountid = (
						SELECT accountid
						FROM PUBLIC.session
						WHERE sessionid = @sessionid
						)
					AND mstatus = 1
				ORDER BY sessionid ASC

			");
			selectparm.Add("@sessionid", request.sessionid);
			selectdt.Load(cn.ExecuteReader(querySql.ToString(), selectparm));

			is_equipment_exist=GetEquipment_exist(request.sessionid);

			//表示該裁判有正在判決中的場次
			if (selectdt.Rows.Count > 0&& request.mstatus==1)
            {
				isSuccess = false;
				message = "該裁判目前有正在判決中的場次!";

			}
			else if (is_equipment_exist==false)
            {
				isSuccess = false;
				message = "設未設定設備編號";
			}
			//沒有就新增
            else
            {
				querySql.Clear();

				querySql.Append(@"	
				UPDATE PUBLIC.session
				SET mstatus = @mstatus
				WHERE sessionid = @sessionid
			");

			
				parm.Add("@sessionid", request.sessionid);
				parm.Add("@mstatus", request.mstatus);
				dt.Load(cn.ExecuteReader(querySql.ToString(), parm));
			}


			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = null,
				Count = dt.Rows.Count,
			};
		}


        #endregion

        #region 取得設備編號是否存在
        public bool GetEquipment_exist(int sessionid)
        {
			bool is_equipment_exist = true;
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			var dt = new DataTable();
			#region  sql
			querySql.Append(@"
				SELECT is_equipment_exist(sessionid)
				FROM PUBLIC.session
				WHERE sessionid = @sessionid;
				");
            #endregion
            parm.Add("@sessionid", sessionid);
			dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			is_equipment_exist =Convert.ToBoolean( dt.Rows[0]["is_equipment_exist"].ToString());

			return is_equipment_exist;

		}
		#endregion

		#region For Winform Sessionselect 

		/// <summary>
		/// 取得場次For Winform
		/// </summary>
		/// <param name="tournamentid"></param>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetSessionSelect(int tournamentid)
		{			
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			StringBuilder querySql = new StringBuilder();
			string message = "";
			bool isSuccess = true;
			var parm = new DynamicParameters();
			var dt = new DataTable();
			#region  sql
			querySql.Append(@"
			
					SELECT sessionid, sessionname
					FROM public.session 
					WHERE tournamentid=@tournamentid;
				");
			#endregion
			parm.Add("@tournamentid", tournamentid);

			dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

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
