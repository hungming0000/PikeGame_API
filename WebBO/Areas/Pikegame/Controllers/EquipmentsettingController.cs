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
    public class EquipmentsettingController
    {
        protected ConnectionFactory _connectionFactory;
        public EquipmentsettingController()
        {
            _connectionFactory = new ConnectionFactory();

        }

        #region 取得全部設備編號
        /// <summary>
        /// 取得全部設備編號
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetAllEquipmentsetting()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
     
            querySql.Append(@"
					SELECT * FROM public.equipmentsetting
                    ORDER BY sid ASC 
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

        #region 取得設備編號
        /// <summary>
        /// 取得設備編號
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetEquipmentsetting(int sessionid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();

            querySql.Append(@"
					SELECT *
                    FROM PUBLIC.equipmentsetting
                    WHERE sessionid = @sessionid
                    ORDER BY team
            ");

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

        #region 新增設備編號
        /// <summary>
        /// 新增設備編號
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult SetEquipmentsetting(EquipmentsettingModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            #region  sql
            querySql.Append(@"
				
			INSERT INTO PUBLIC.equipmentsetting (
								sessionid,
								equipmentid,
								team								
							)
						VALUES (
					@sessionid,
					@blue_equipmentid,
					'Blue'			
						);
			INSERT INTO PUBLIC.equipmentsetting (
								sessionid,
								equipmentid,
								team								
							)
						VALUES (
					@sessionid,
					@red_equipmentid,
					'Red'			
						);
			       ");
            #endregion
            parm.Add("@sessionid", request.sessionid);
            parm.Add("@blue_equipmentid", request.blue_equipmentid);
            parm.Add("@red_equipmentid", request.red_equipmentid);

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

        #region 編輯設備編號
        /// <summary>
        /// 編輯設備編號
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult EditEquipmentsetting(EquipmentsettingModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            #region  sql
            querySql.Append(@"
				UPDATE PUBLIC.equipmentsetting
                SET equipmentid = @blue_equipmentid
                WHERE sessionid = @sessionid
	                AND team = 'Blue';

                UPDATE PUBLIC.equipmentsetting
                SET equipmentid = @red_equipmentid
                WHERE sessionid = @sessionid
	                AND team = 'Red';
			
			       ");
            #endregion
            parm.Add("@sessionid", request.sessionid);
            parm.Add("@blue_equipmentid", request.blue_equipmentid);
            parm.Add("@red_equipmentid", request.red_equipmentid);

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
