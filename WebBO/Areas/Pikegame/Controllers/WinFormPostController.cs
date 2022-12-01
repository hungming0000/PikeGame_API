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
     public class WinFormPostController
    {
        protected ConnectionFactory _connectionFactory;
        public WinFormPostController()
        {
            _connectionFactory = new ConnectionFactory();

        }
        #region 智慧槍頭擊中SetAPI For Winform
        /// <summary>
        /// 智慧槍頭擊中SetAPI
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult SetWisdomGunhead(WisdomGunheadModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();
            bool isstart = false;
            isstart= checkSession(request.sessionid);

            if(isstart==true)
            { 

            querySql.Append(@"
					INSERT INTO public.sessiondetial(sessionid,sessiondetialtime,redhit,bluehit, redfraction, bluefraction, mstatus)
					VALUES(
                    @sessionid,now(),@redhit,@bluehit,@redfraction,@bluefraction,@mstatus
                    ) 
					
			");

            parm.Add("@sessionid", request.sessionid);
            parm.Add("@redhit", request.redhit);
            parm.Add("@bluehit", request.bluehit);
            parm.Add("@redfraction", 0);
            parm.Add("@bluefraction", 0);
            parm.Add("@mstatus", 1);

            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));
            message = "槍頭擊中!";
            }
            else
            {
                isSuccess = false;
                message = "該場次尚未開始比賽或是已結束。";
            }

            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = dt,
                Count = dt.Rows.Count,
            };
        }
        #endregion

        #region 檢查該場次是否已經開始比賽
        public bool checkSession(int sessionid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            bool isstart = false;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();
            int mstatus = 0;

            querySql.Append(@"
					SELECT mstatus
					FROM public.session 
					WHERE sessionid=@sessionid
					
			");
            parm.Add("@sessionid", sessionid);           

            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));
            if (dt.Rows.Count > 0) { 
            mstatus= dt.Rows[0]["mstatus"].ToString()!=null?Convert.ToInt32(dt.Rows[0]["mstatus"].ToString()):0;
            }
            switch (mstatus)
            {
                case 0:
                    isstart=false;
                    break;
                case 1:
                    isstart=true;
                    break;
                case 2:
                    isstart = false;
                    break;
                default:
                    isstart = false;
                    break;
            }


            return isstart;
        }

        #endregion


        #region For testapi   TournamentSelect
        /// <summary>
        /// 取得比賽For testapi
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


        #region For testapi Sessionselect 

        /// <summary>
        /// 取得場次For testapi
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
			
					SELECT sessionid, sessionname
					FROM public.session 
					WHERE tournamentid=@tournamentid AND mstatus=1;
				");
            #endregion
            parm.Add("@tournamentid", Convert.ToInt32(tournamentid));

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
