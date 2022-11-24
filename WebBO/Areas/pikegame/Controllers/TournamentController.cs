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
    public class TournamentController
    {
        protected ConnectionFactory _connectionFactory;
        public TournamentController()
        {
            _connectionFactory = new ConnectionFactory();

        }
        #region 取得比賽列表
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
            #region old sql
            //       querySql.Append(@"
            //        SELECT t.*,
            //	(
            //		SELECT accountname
            //		FROM accountm AS a
            //		WHERE a.accountid = s.accountid
            //		) jugde,
            //	(
            //		SELECT accountname
            //		FROM accountm AS a
            //		WHERE a.accountid = s.red_accountid
            //		) red_account,
            //	(
            //		SELECT accountname
            //		FROM accountm AS a
            //		WHERE a.accountid = s.blue_accountid
            //		) blue_account,
            //	s.sessionname,
            //	s.sessiontime,
            //	s.redfraction_sum,
            //	s.bluefraction_sum,
            //	s.mstatus,
            //	s.sessionid
            //FROM PUBLIC.session AS s
            //INNER JOIN PUBLIC.tournament AS t ON s.tournamentid = t.tournamentid
            //ORDER BY tournamentstartdate DESC

            //       ");
            #endregion

            querySql.Append(@"
					SELECT tournamentid,
						tournamentname,
						(tournamentstartdate::TEXT) || '~' || (tournamentenddate::TEXT) tournamentdate,
						maxfraction					
					FROM PUBLIC.tournament
					ORDER BY tournamentstartdate DESC
			");


            var dt = new DataTable();
            dt.Load(cn.ExecuteReader(querySql.ToString()));

            //dt.Columns.Add(new DataColumn("Item", typeof(SessionModel)));
            //var SessionData=  new SessionController().GetSession();


            //         for (var i = 0; i < dt.Rows.Count; i++)
            //         {
            //             for (var d = 0; d < SessionData.Rows.Count; d++)
            //             {
            //                 if (dt.Rows[i]["tournamentid"].ToString() == SessionData.Rows[d]["tournamentid"].ToString())
            //                 {					
            //			SessionModel session = new SessionModel();
            //			session.sessionid=Convert.ToInt32(SessionData.Rows[d]["sessionid"].ToString());
            //			session.tournamentid = Convert.ToInt32(SessionData.Rows[d]["tournamentid"].ToString());
            //			session.judge_account = SessionData.Rows[d]["judge_account"].ToString();
            //			session.sessionname = SessionData.Rows[d]["sessionname"].ToString();
            //			session.sessiontime = Convert.ToDateTime( SessionData.Rows[d]["sessiontime"]);
            //			session.red_account = SessionData.Rows[d]["red_account"].ToString();
            //			session.blue_account = SessionData.Rows[d]["blue_account"].ToString();
            //			session.redfraction_sum = Int16.Parse(SessionData.Rows[d]["redfraction_sum"].ToString());
            //			session.bluefraction_sum = Int16.Parse(SessionData.Rows[d]["bluefraction_sum"].ToString());
            //			session.blue_accountid = SessionData.Rows[d]["mstatus"].ToString();

            //			dt.Rows[i]["Item"] = session;
            //			//row["Item"] = session;
            //			//dt.Rows.Add(row);	
            //		}
            //             }
            //         }

            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = dt,
                Count = dt.Rows.Count,
            };
        }
        #endregion

        #region 取得選手列表
        /// <summary>
        /// 取得選手列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetPlayerAccount()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
			    SELECT accountid,accountname
				FROM accountm
				WHERE accountgroupname = '選手'

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

        #region 取得裁判列表
        /// <summary>
        /// 取得裁判列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetJudgeAccount()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
			    SELECT accountid,accountname
				FROM accountm
				WHERE accountgroupname = '裁判'

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

        #region  取得現在比賽的場次
        /// <summary>
        /// 取得現在比賽的場次
        /// </summary>
        /// <returns></returns>
        public DataTable GetNowTournament()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
			    SELECT *
				FROM PUBLIC.tournament
				WHERE tournamentstartdate <= now()
					AND tournamentEnddate >= now()

            ");

            var dt = new DataTable();
            dt.Load(cn.ExecuteReader(querySql.ToString()));

            return dt;
        }

        public DataTable GetNowTournamentByequipmentid(string equipmentid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            querySql.Append(@"
						SELECT *
						FROM PUBLIC.equipmentsetting
						WHERE sessionid = (
								SELECT sessionid
								FROM PUBLIC.session
								WHERE sessionid IN (
										SELECT sessionid
										FROM PUBLIC.equipmentsetting
										WHERE equipmentid =@equipmentid
										)
									AND mstatus = 1
								)
							AND equipmentid = @equipmentid			
            ");

            var dt = new DataTable();
            parm.Add("@equipmentid", equipmentid);
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            return dt;
        }
        #endregion

        #region 新增比賽列表
        /// <summary>
        /// 新增比賽列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult SetTournament(TournamentModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            #region old sql
            //       querySql.Append(@"

            //	INSERT INTO PUBLIC.tournament (
            //		tournamentname,
            //		tournamentstartdate,
            //		tournamentenddate,
            //		maxfraction
            //		)
            //	VALUES (
            //	@tournamentname,
            //	@tournamentstartdate,
            //	@tournamentenddate,
            //	@maxfraction
            //	);
            //INSERT INTO PUBLIC.session (
            //					tournamentid,
            //					accountid,
            //					sessionname,
            //					red_accountid,
            //					blue_accountid,
            //					redfraction_sum,
            //					bluefraction_sum,
            //					mstatus
            //				)
            //			VALUES (
            //		(SELECT currval('tournament_tournamentid_seq')),
            //		@judge_accountid,
            //		@sessionname,
            //		@red_accountid,
            //		@blue_accountid,
            //		@redfraction_sum,
            //		@bluefraction_sum,
            //		@mstatus
            //			);
            //       ");
            #endregion


            querySql.Append(@"	
			INSERT INTO PUBLIC.tournament (
							tournamentname,
							tournamentstartdate,
							tournamentenddate,
							maxfraction
							)
						VALUES (
						@tournamentname,
						@tournamentstartdate,
						@tournamentenddate,
						@maxfraction
						);");

            var dt = new DataTable();
            parm.Add("@tournamentname", request.tournamentname);
            parm.Add("@tournamentstartdate", Convert.ToDateTime(request.tournamentstartdate));
            parm.Add("@tournamentenddate", Convert.ToDateTime(request.tournamentenddate));
            parm.Add("@maxfraction", request.maxfraction);

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



        /// <summary>
        /// 取得細項
        /// </summary>
        /// <returns></returns>

        #region For Winform Sessionselect 

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
			
				SELECT tournamentid, tournamentname 
                FROM public.tournament 
                ORDER BY tournamentstartdate DESC ;
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


    }
}
