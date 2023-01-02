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
   public class MatchScoreController
    {
        protected ConnectionFactory _connectionFactory;
        public MatchScoreController()
        {
            _connectionFactory = new ConnectionFactory();
        }

		#region 取得場次資訊
		/// <summary>
		/// 取得場次資訊
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetSessionScoreById(int sessionid)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			var parm = new DynamicParameters();
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"		         
					SELECT * ,
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
					FROM public.session s
					WHERE sessionid=@sessionid
					ORDER BY sessionid desc 
            ");

			var dt = new DataTable();
			parm.Add("@sessionid", sessionid);
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
