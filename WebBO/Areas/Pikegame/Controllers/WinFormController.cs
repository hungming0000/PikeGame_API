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
    public class WinFormController
    {
        protected ConnectionFactory _connectionFactory;
        public WinFormController()
        {
            _connectionFactory = new ConnectionFactory();
        }

        #region For Winform TournamentSelect
        /// <summary>
        /// 取得比賽For Winform
        /// </summary>       
        /// <returns></returns>
        public ExecuteCommandAPIResult GetTournamentSelect()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            StringBuilder querySql = new StringBuilder();
            string message = "";
            bool isSuccess = true;
            var dt = new DataTable();
            #region  sql
            querySql.Append(@"
			
				SELECT tournamentid,
	                            tournamentname,
								maxfraction
                            FROM PUBLIC.tournament
                WHERE tournamentid IN (
		                SELECT tournamentid
		                FROM PUBLIC.session
		                WHERE mstatus = '1'
		                )
                ORDER BY tournamentstartdate DESC;

				");
            #endregion         

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


        #region For Winform Sessionselect 

        /// <summary>
        /// 取得場次For Winform
        /// </summary>
        /// <param name="tournamentid"></param>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetSessionSelect(string tournamentid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            StringBuilder querySql = new StringBuilder();
            string message = "";
            bool isSuccess = true;
            var parm = new DynamicParameters();
            var dt = new DataTable();
            #region  sql
            querySql.Append(@"
			
					SELECT sessionid,
	                                sessionname
                     FROM PUBLIC.session
                    WHERE mstatus = '1'
	                                AND tournamentid = @tournamentid;

				");
            #endregion
            parm.Add("@tournamentid", Convert.ToInt32( tournamentid));

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
