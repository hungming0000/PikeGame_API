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
   public  class SessiondetialController
    {
        protected ConnectionFactory _connectionFactory;
        public SessiondetialController()
        {
            _connectionFactory = new ConnectionFactory();

        }

		/// <summary>
		/// 取得比賽列表
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetSessiondetail(int sessionid)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			StringBuilder querySql2 = new StringBuilder();
			var parm = new DynamicParameters();
			var dt = new DataTable();
			var resultdt = new DataTable();

			querySql.Append(String.Format(@"
					SELECT pivotcode('(select red_accountid as player,sessiondetialid  ,redfraction
					from session t1
					left join  sessiondetial  t2 on  t1.sessionid = t2.sessionid
					where t1.sessionid ={0}
					union
					select blue_accountid,sessiondetialid  ,bluefraction
					from session t1
					left join  sessiondetial  t2 on  t1.sessionid = t2.sessionid
					where t1.sessionid ={0}
					order by 1) a', 'player', 'sessiondetialid', 'redfraction', 'integer');			          
            ", sessionid));
			
			dt.Load(cn.ExecuteReader(querySql.ToString()));


			//如果無資料則顯示選手名單
            if (dt.Rows[0]["pivotcode"].ToString() == "")
            {
				querySql2.Append(@"
						SELECT red_accountid AS player
						FROM session t1
						LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
						WHERE t1.sessionid =@sessionid
						UNION
						SELECT blue_accountid
						FROM session t1
						LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
						WHERE t1.sessionid = @sessionid
						ORDER BY 1");

				parm.Add("@sessionid", sessionid);
				resultdt.Load(cn.ExecuteReader(querySql2.ToString(),parm));
			}
            else
            {
				querySql2.Append(dt.Rows[0]["pivotcode"].ToString());
				resultdt.Load(cn.ExecuteReader(querySql2.ToString()));

			}
			

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = resultdt,
				Count = resultdt.Rows.Count,
			};
		}


		/// <summary>
		/// 取得比賽回合
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetSessionRound(int sessionid)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			var dt = new DataTable();

			querySql.Append(@"
					SELECT DISTINCT max(sessiondetialid)
					FROM (
						SELECT red_accountid AS accountid,
							sessiondetialid,
							redfraction
						FROM session t1
						LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
						WHERE t1.sessionid = @sessionid
	
						UNION
	
						SELECT blue_accountid,
							sessiondetialid,
							bluefraction
						FROM session t1
						LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
						WHERE t1.sessionid = @sessionid
						ORDER BY 1
						) a
					ORDER BY 1
            ");

			parm.Add("@sessionid", sessionid);
			dt.Load(cn.ExecuteReader(querySql.ToString(), parm));		



			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt.Rows[0]["max"].ToString(),
				Count = 1,
			};
		}
		/// <summary>
		/// 智慧槍頭擊中GetApi
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult SetWisdomGunhead()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			var dt = new DataTable();

			#region 取得現在場次的ID

			DataTable TournamentDt =  new TournamentController().GetNowTournament();

			var tournamentid = TournamentDt.Rows[0]["tournamentid"].ToString();

			#endregion

			querySql.Append(@"
					SELECT *
					FROM session
					WHERE tournamentid = @tournamentid
			");
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

		public ExecuteCommandAPIResult GetWisdomGunhead()
        {
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			var dt = new DataTable();






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
