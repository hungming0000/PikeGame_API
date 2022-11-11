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
            StringBuilder querySql = new StringBuilder();
            StringBuilder querySql2 = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();
            var resultdt = new DataTable();

            querySql.Append(String.Format(@"
					SELECT pivotcode('(select red_accountid as player, row_number() over (ORDER BY sessiondetialid)as rownum  ,redfraction
					from session t1
					left join  sessiondetial  t2 on  t1.sessionid = t2.sessionid
					where t1.sessionid ={0}
					union
					select blue_accountid,row_number() over (ORDER BY sessiondetialid)as rownum  ,bluefraction
					from session t1
					left join  sessiondetial  t2 on  t1.sessionid = t2.sessionid
					where t1.sessionid ={0}
					order by rownum) a', 'player', 'rownum', 'redfraction', 'integer');			          
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
                resultdt.Load(cn.ExecuteReader(querySql2.ToString(), parm));
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
        #endregion

        #region 智慧槍頭擊中function 

        public ExecuteCommandAPIResult SetSessiondetial(int sessionid,string team)
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
            #region  sql
            querySql.Append(@"
				UPDATE PUBLIC.sessiondetial
                SET redfraction =@redfraction,
	                bluefraction=@bluefraction,
                    mstatus=0
                WHERE sessiondetialid =@sessiondetialid ;

                    UPDATE PUBLIC.session
                    SET redfraction_sum = redfraction_sum +@redfraction,
	                    bluefraction_sum = bluefraction_sum +@bluefraction
                    WHERE sessionid = @sessionid ;

			
			       ");
            #endregion            
            parm.Add("@sessionid", request.sessionid);
            parm.Add("@sessiondetialid", request.sessiondetialid);
            parm.Add("@redfraction", request.redfraction);
            parm.Add("@bluefraction", request.bluefraction);

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

    }
}
