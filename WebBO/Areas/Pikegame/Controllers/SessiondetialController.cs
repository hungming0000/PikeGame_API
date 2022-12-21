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
    public class SessiondetialController
    {
        protected ConnectionFactory _connectionFactory;
        public SessiondetialController()
        {
            _connectionFactory = new ConnectionFactory();

        }

        #region 取得分數列表
        /// <summary>
        /// 取得分數列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetSessiondetail(int sessionid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder scorequerySql = new StringBuilder();
            StringBuilder sessionquerySql = new StringBuilder();
            StringBuilder Uniontablesql = new StringBuilder();
            StringBuilder scoretablesql = new StringBuilder();
            StringBuilder sessiontablesql = new StringBuilder();

            var parm = new DynamicParameters();
            var scoredt = new DataTable();
            var sessiondt = new DataTable();
            var resultdt = new DataTable();

            scorequerySql.Append(String.Format(@"
					    DELETE
                        FROM tempscoretable;

                        INSERT INTO tempscoretable
                        SELECT (
		                        SELECT accountname
		                        FROM accountm AS a
		                        WHERE a.accountid = t1.red_accountid
		                        ) AS player,
	                        row_number() OVER (
		                        ORDER BY sessiondetialid
		                        ) AS rownum,
	                        CASE redfraction
		                        WHEN 0
			                        THEN '0'
		                        ELSE redfraction || '(' || CASE redhitposition
				                        WHEN '0'
					                        THEN '頭'
				                        WHEN '1'
					                        THEN '軀幹'
				                        WHEN '2'
					                        THEN '四肢'
				                        WHEN '3'
					                        THEN '掉槍'
				                        END || ')'
		                        END AS redfraction
                        FROM session t1
                        LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
                        WHERE t1.sessionid = {0}

                        UNION

                        SELECT (
		                        SELECT accountname
		                        FROM accountm AS a
		                        WHERE a.accountid = t1.blue_accountid
		                        ) AS player,
	                        row_number() OVER (
		                        ORDER BY sessiondetialid
		                        ) AS rownum,
	                        CASE bluefraction
		                        WHEN 0
			                        THEN '0'
		                        ELSE bluefraction || '(' || CASE bluehitposition
				                        WHEN '0'
					                        THEN '頭'
				                        WHEN '1'
					                        THEN '軀幹'
				                        WHEN '2'
					                        THEN '四肢'
				                        WHEN '3'
					                        THEN '掉槍'
				                        END || ')'
		                        END AS bluefraction
                        FROM session t1
                        LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
                        WHERE t1.sessionid ={0}
                        ORDER BY rownum;

                        SELECT pivotcode('(select  player,  rownum  ,fraction 
					                        from tempscoretable 
					
					
					                        ) a', 'player', 'rownum', 'fraction', 'text');

            ", sessionid));

            scoredt.Load(cn.ExecuteReader(scorequerySql.ToString()));//回合表

            sessionquerySql.Append(String.Format(@"
					   DELETE
                        FROM tempsessiontable;

                        INSERT INTO tempsessiontable
                        SELECT 'id' AS id,
	                        row_number() OVER (
		                        ORDER BY sessiondetialid
		                        ) AS rownum,
	                        sessiondetialid
                        FROM session t1
                        LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
                        WHERE t1.sessionid = {0}
                        ORDER BY rownum;

                        SELECT pivotcode('(select  player,  rownum  ,fraction 
					                                                from tempsessiontable 
					                                                ) a', 'player', 'rownum', 'fraction', 'text');


            ", sessionid));

            sessiondt.Load(cn.ExecuteReader(sessionquerySql.ToString()));//ID表
            //如果無資料則顯示選手名單
            if (scoredt.Rows[0]["pivotcode"].ToString() == "")
            {
                scoretablesql.Append(@"
						SELECT (
		                SELECT accountname
		                FROM accountm AS a
		                WHERE a.accountid = t1.blue_accountid
		                )  AS player
						FROM session t1
						LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
						WHERE t1.sessionid =@sessionid
						UNION
						SELECT  (
		                SELECT accountname
		                FROM accountm AS a
		                WHERE a.accountid = t1.red_accountid
		                ) 
						FROM session t1
						LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
						WHERE t1.sessionid = @sessionid
						ORDER BY 1");

                parm.Add("@sessionid", sessionid);
                resultdt.Load(cn.ExecuteReader(scoretablesql.ToString(), parm));
            }
            else
            {
                scoretablesql.Append(scoredt.Rows[0]["pivotcode"].ToString());//回合表

                sessiontablesql.Append(sessiondt.Rows[0]["pivotcode"].ToString());//ID表

                Uniontablesql.Append(String.Format(@"
                    	WITH
                       sessiontable AS (
	                    {0}
                     ),
                     scoretable AS(
                        {1}
                     )
 
 
                     select *from sessiontable
                     union
                     select *from scoretable 
                     order by 選手 desc
                    ;
                ", sessiontablesql.Remove(sessiontablesql.Length - 1, 1) , scoretablesql.Remove(scoretablesql.Length - 1, 1)));

                resultdt.Load(cn.ExecuteReader(Uniontablesql.ToString()));

            }


            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = resultdt,
                Count = resultdt.Rows.Count,
            };
        }
        #endregion

        #region 取得分數列表細項
        /// <summary>
        /// 取得分數列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetSessiondetailById(int sessiondetialid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();    
            var dt = new DataTable();
            var parm = new DynamicParameters();

            querySql.Append(@"
                      SELECT sessiondetialid,
	                                sessionid,
	                                redfraction,
	                                bluefraction,
	                                finalredfraction,
	                                finalbluefraction,
	                                redhitposition,
	                                bluehitposition,
	                                CASE redhitposition
		                                WHEN '0'
			                                THEN '頭'
		                                WHEN '1'
			                                THEN '軀幹'
		                                WHEN '2'
			                                THEN '四肢'
		                                WHEN '3'
			                                THEN '掉槍'
		                                END redhitpositionVal,
	                                CASE bluehitposition
		                                WHEN '0'
			                                THEN '頭'
		                                WHEN '1'
			                                THEN '軀幹'
		                                WHEN '2'
			                                THEN '四肢'
		                                WHEN '3'
			                                THEN '掉槍'
		                                END bluehitpositionVal
                                FROM sessiondetial
                        WHERE sessiondetialid = @sessiondetialid
					");

            parm.Add("@sessiondetialid", sessiondetialid);
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

        #region 取得分數列表(舊)
        /// <summary>
        /// 取得分數列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetSessiondetail_old(int sessionid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            StringBuilder scoretablesql = new StringBuilder();
            StringBuilder sessiontablesql = new StringBuilder();

            var parm = new DynamicParameters();
            var dt = new DataTable();
            var resultdt = new DataTable();

            querySql.Append(String.Format(@"
					SELECT pivotcode('(select (
		            SELECT accountname
		            FROM accountm AS a
		            WHERE a.accountid = t1.red_accountid
		            ) AS player, row_number() over (ORDER BY sessiondetialid)as rownum  ,redfraction
					from session t1
					left join  sessiondetial  t2 on  t1.sessionid = t2.sessionid
					where t1.sessionid ={0}
					union
					select (
		            SELECT accountname
		            FROM accountm AS a
		            WHERE a.accountid = t1.blue_accountid
		            ) AS player ,row_number() over (ORDER BY sessiondetialid)as rownum  ,bluefraction
					from session t1
					left join  sessiondetial  t2 on  t1.sessionid = t2.sessionid
					where t1.sessionid ={0}
					order by rownum) a', 'player', 'rownum', 'redfraction', 'integer');			          
            ", sessionid));

            dt.Load(cn.ExecuteReader(querySql.ToString()));


            //如果無資料則顯示選手名單
            if (dt.Rows[0]["pivotcode"].ToString() == "")
            {
                scoretablesql.Append(@"
						SELECT (
		                SELECT accountname
		                FROM accountm AS a
		                WHERE a.accountid = t1.blue_accountid
		                )  AS player
						FROM session t1
						LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
						WHERE t1.sessionid =@sessionid
						UNION
						SELECT  (
		                SELECT accountname
		                FROM accountm AS a
		                WHERE a.accountid = t1.red_accountid
		                ) 
						FROM session t1
						LEFT JOIN sessiondetial t2 ON t1.sessionid = t2.sessionid
						WHERE t1.sessionid = @sessionid
						ORDER BY 1");

                parm.Add("@sessionid", sessionid);
                resultdt.Load(cn.ExecuteReader(scoretablesql.ToString(), parm));
            }
            else
            {
                scoretablesql.Append(dt.Rows[0]["pivotcode"].ToString());
               

                resultdt.Load(cn.ExecuteReader(scoretablesql.ToString()));

            }


            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = resultdt,
                Count = resultdt.Rows.Count,
            };
        }
        #endregion

        #region 智慧槍頭擊中function 

        public ExecuteCommandAPIResult SetSessiondetial(int sessionid, string team)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();
            bool bluehit = true;
            bool redhit = true;

            if (team == "Blue")
            {
                bluehit = true;
                redhit = false;
            }
            else
            {
                redhit = true;
                bluehit = false;
            }

            querySql.Append(@"
					INSERT INTO public.sessiondetial(sessionid,sessiondetialtime,redhit,bluehit,mstatus)
					VALUES(
                    @sessionid,now(),@redhit,@bluehit,@mstatus
                    ) 
					
			");
            parm.Add("@sessionid", sessionid);
            parm.Add("@redhit", redhit);
            parm.Add("@bluehit", bluehit);
            parm.Add("@mstatus", 1);

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

        #region 智慧槍頭擊中GetApi
        /// <summary>
        /// 智慧槍頭擊中GetApi
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult SetWisdomGunhead(string equipmentid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();

            #region 取得現在場次的ID

            DataTable SessionDt = new TournamentController().GetNowTournamentByequipmentid(equipmentid);

            var sessionid = Convert.ToInt32(SessionDt.Rows[0]["sessionid"].ToString());
            var team = SessionDt.Rows[0]["team"].ToString();

            #endregion

            #region 擊中槍頭function
            return SetSessiondetial(sessionid, team);
            #endregion        
        }
        //public ExecuteCommandAPIResult SetWisdomGunhead()
        //{
        //	IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
        //	string message = "";
        //	bool isSuccess = true;
        //	StringBuilder querySql = new StringBuilder();
        //	var parm = new DynamicParameters();
        //	var dt = new DataTable();

        //	#region 取得現在場次的ID

        //	DataTable TournamentDt =  new TournamentController().GetNowTournament();

        //	var tournamentid = TournamentDt.Rows[0]["tournamentid"].ToString();

        //	#endregion

        //	querySql.Append(@"
        //			SELECT *
        //			FROM session
        //			WHERE tournamentid = @tournamentid
        //	");
        //	parm.Add("@tournamentid", tournamentid);
        //	dt.Load(cn.ExecuteReader(querySql.ToString(), parm));


        //	return new ExecuteCommandAPIResult()
        //	{
        //		isSuccess = isSuccess,
        //		Message = message,
        //		Data = dt,
        //		Count = dt.Rows.Count,
        //	};
        //}
        #endregion

        #region 取得擊中的槍頭資料
        /// <summary>
        /// 取得該裁判目前進行中的場次槍頭擊中資料
        /// </summary>
        /// <returns></returns>
        public DataTable GetWisdomGunhead(string judge_accountid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();


            querySql.Append(@"

                    SELECT * FROM public.sessiondetial
                    where sessionid= (SELECT sessionid FROM public.session
                    where accountid=@judge_accountid
				                      and mstatus=1
                    )
                    and mstatus=1
                    limit 1

                ");
            parm.Add("@judge_accountid", judge_accountid);
            var dt = new DataTable();
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            return dt;
        }
        #endregion

        #region 裁判輸入分數

        /// <summary>
        /// 裁判輸入分數
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult EditFraction(SessiondetialModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();

            #region 擊中部位-分數
            switch (request.redhitposition)
            {
                case "0"://頭
                    request.redfraction = 3;
                    request.finalredfraction = 3;
                    request.redhitposition = "0";
                        break;
                case "1"://軀幹
                    request.redfraction = 3;
                    request.finalredfraction = 3;
                    request.redhitposition = "1";
                    break;
                case "2"://四肢
                    request.redfraction = 1;
                    request.finalredfraction = 1;
                    request.redhitposition = "2";
                    break;
                case "3"://掉槍
                    request.redfraction = -1;
                    request.finalredfraction = -1;
                    request.redhitposition = "3";
                    break;
                default:
                    request.redfraction = 0;
                    request.finalredfraction = 0;
                    request.redhitposition = (string)null;
                    break;
            }
            switch (request.bluehitposition)
            {
                case "0": //頭
                    request.bluefraction = 3;
                    request.finalbluefraction = 3;
                    request.bluehitposition = "0";
                    break;
                case "1": //軀幹
                    request.bluefraction = 3;
                    request.finalbluefraction = 3;
                    request.bluehitposition = "1";
                    break;
                case "2": //四肢
                    request.bluefraction = 1;
                    request.finalbluefraction = 1;
                    request.bluehitposition = "2";
                    break;
                case "3": //掉槍
                    request.bluefraction = -1;
                    request.finalbluefraction = -1;
                    request.bluehitposition = "3";
                    break;
                default:
                    request.bluefraction = 0;
                    request.finalbluefraction = 0;
                    request.bluehitposition = (string)null;
                    break;

            }
            #endregion

            #region  sql
            querySql.Append(@"
				UPDATE PUBLIC.sessiondetial
                SET  redfraction =@redfraction,
	                     bluefraction=@bluefraction,
                         finalredfraction = @redfraction,
	                     finalbluefraction = @bluefraction,
                         bluehitposition=@bluehitposition,
                         redhitposition=@redhitposition,
                    mstatus=0
                WHERE sessiondetialid =@sessiondetialid ;

                    UPDATE PUBLIC.session
                    SET 
                        redfraction_sum = redfraction_sum +@redfraction,
	                    bluefraction_sum = bluefraction_sum +@bluefraction               
                    WHERE sessionid = @sessionid ;

			
			       ");
            #endregion            
            parm.Add("@sessionid", request.sessionid);
            parm.Add("@sessiondetialid", request.sessiondetialid);
            parm.Add("@redfraction", request.redfraction);
            parm.Add("@bluefraction", request.bluefraction);
            parm.Add("@finalredfraction", request.finalredfraction);
            parm.Add("@finalbluefraction", request.finalbluefraction);
            parm.Add("@redhitposition", request.redhitposition);
            parm.Add("@bluehitposition", request.bluehitposition);

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


        #region 裁判編輯分數

        /// <summary>
        /// 裁判編輯分數
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult EditFractionbyId(SessiondetialModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            StringBuilder scorequerySql = new StringBuilder();
            var parm = new DynamicParameters();
            var scoreparm = new DynamicParameters();
            var scoredt = new DataTable();
            int orginBscore = 0;
            int orginRscore = 0;
            int diffBscore = 0;
            int diffRscore = 0;

            #region 擊中部位-分數
            switch (request.redhitposition)
            {
                case "0":
                    request.redfraction = 3;
                    request.finalredfraction = 3;
                    request.redhitposition = "0";
                    break;
                case "1":
                    request.redfraction = 3;
                    request.finalredfraction = 3;
                    request.redhitposition = "1";
                    break;
                case "2":
                    request.redfraction = 1;
                    request.finalredfraction = 1;
                    request.redhitposition = "2";
                    break;
                case "3":
                    request.redfraction = -1;
                    request.finalredfraction = -1;
                    request.redhitposition = "3";
                    break;
                default:
                    request.redfraction = 0;
                    request.finalredfraction = 0;
                    request.redhitposition = (string)null;
                    break;
            }
            switch (request.bluehitposition)
            {
                case "0":
                    request.bluefraction = 3;
                    request.finalbluefraction = 3;
                    request.bluehitposition = "0";
                    break;
                case "1":
                    request.bluefraction = 3;
                    request.finalbluefraction = 3;
                    request.bluehitposition = "1";
                    break;
                case "2":
                    request.bluefraction = 1;
                    request.finalbluefraction = 1;
                    request.bluehitposition = "2";
                    break;
                case "3":
                    request.bluefraction = -1;
                    request.finalbluefraction = -1;
                    request.bluehitposition = "3";
                    break;
                default:
                    request.bluefraction = 0;
                    request.finalbluefraction = 0;
                    request.bluehitposition = (string)null;
                    break;
            }
            #endregion


            #region 取得原始分數
            scorequerySql.Append(@"
                SELECT finalbluefraction,finalredfraction
                FROM public.sessiondetial
                where sessiondetialid=@sessiondetialid
                ORDER BY sessiondetialid ASC             
                ");

            #endregion
            scoreparm.Add("@sessiondetialid", request.sessiondetialid);
            scoredt.Load(cn.ExecuteReader(scorequerySql.ToString(), scoreparm));

            orginBscore =Convert.ToInt32( scoredt.Rows[0]["finalbluefraction"].ToString());
            orginRscore = Convert.ToInt32(scoredt.Rows[0]["finalredfraction"].ToString());

            diffBscore = request.bluefraction.Value - orginBscore;
            diffRscore = request.redfraction.Value - orginRscore;

            #region  sql
            querySql.Append(@"

                UPDATE PUBLIC.sessiondetial
                SET  redfraction =@redfraction,
	                     bluefraction=@bluefraction,
                         finalredfraction =@redfraction,
	                     finalbluefraction = @bluefraction,
                         bluehitposition=@bluehitposition,
                         redhitposition=@redhitposition,
                    mstatus=0
                WHERE sessiondetialid =@sessiondetialid ;

                    UPDATE PUBLIC.session
                    SET 
                        redfraction_sum = redfraction_sum +@diffRscore,
	                    bluefraction_sum = bluefraction_sum +@diffBscore            
                    WHERE sessionid = @sessionid ;

			
			       ");
            #endregion            
            parm.Add("@sessionid", request.sessionid);
            parm.Add("@sessiondetialid", request.sessiondetialid);
            parm.Add("@redfraction", request.redfraction);
            parm.Add("@bluefraction", request.bluefraction);
            parm.Add("@finalredfraction", request.finalredfraction);
            parm.Add("@finalbluefraction", request.finalbluefraction);
            parm.Add("@redhitposition", request.redhitposition);
            parm.Add("@bluehitposition", request.bluehitposition);
            parm.Add("@diffRscore", diffRscore);
            parm.Add("@diffBscore", diffBscore);

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

        #region 取得機器編號 
        /// <summary>
		/// 取得機器編號 
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetEquipmentidList()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"			         
					SELECT sessionname,
	                (
		                SELECT accountname
		                FROM accountm AS a
		                WHERE a.accountid = s.accountid
		                ) judge_account,
                    team,
	                equipmentid
                FROM PUBLIC.equipmentsetting e
                LEFT JOIN PUBLIC.session s ON e.sessionid = s.sessionid
                WHERE mstatus=1
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

    }
}
