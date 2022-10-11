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
    public class TournamentController
    {
        protected ConnectionFactory _connectionFactory;
        public TournamentController()
        {
            _connectionFactory = new ConnectionFactory();

        }
		/// <summary>
		/// 取得比賽列表
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetTournament()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
			          SELECT t.*,
						(
							SELECT accountname
							FROM accountm AS a
							WHERE a.accountid = s.accountid
							) jugde,
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
						s.sessionname,
						s.sessiontime,
						s.redfraction_sum,
						s.bluefraction_sum,
						s.mstatus
					FROM PUBLIC.session AS s
					INNER JOIN PUBLIC.tournament AS t ON s.tournamentid = t.tournamentid

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
