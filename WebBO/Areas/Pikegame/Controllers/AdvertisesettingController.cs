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
   public  class AdvertisesettingController
    {
        protected ConnectionFactory _connectionFactory;
        public AdvertisesettingController()
        {
            _connectionFactory = new ConnectionFactory();
        }

        #region 取得廣告輪播列表
        /// <summary>
        /// 取得廣告輪播列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetAdvertisesetting()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;

			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			         
					SELECT  advertiseid,
									advertiseurl,
									case adsstatus when 0 then '下架' when 1 then '上架' end adsstatus,
									advertisstarttime::TEXT,
									advertisendtime::TEXT,
									advertistimeperiod,
									advertiscosts,
									modifydate::TEXT,
									modifyuser,
									periodtimes
								FROM PUBLIC.advertisesetting
								WHERE is_timeperiod_exist(advertiseid, periodtimes) = true

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

		#region 取得廣告輪播列表Byid
		/// <summary>
		/// 取得廣告輪播列表
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetAdvertisesettingById(int advertiseid)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			var parm = new DynamicParameters();
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			         
					SELECT  advertiseid,
									advertiseurl,
									adsstatus,
									advertisstarttime,
									advertisendtime,
									advertistimeperiod,
									advertiscosts,
									modifydate::TEXT,
									modifyuser,
									periodtimes
								FROM PUBLIC.advertisesetting
								WHERE advertiseid = @advertiseid

            ");

			var dt = new DataTable();
			parm.Add("@advertiseid", advertiseid);
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

		#region 新增廣告

		/// <summary>
		/// 新增廣告
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult CreateAdvertise(AdvertisesettingModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
			string advertistimeperiod = request.advertistimeperiod;

			string[] arrays = advertistimeperiod.Split(',');
			request.periodtimes = arrays.Length;


			#region  sql
			querySql.Append(@"

							INSERT INTO PUBLIC.advertisesetting (
								advertiseurl,
								adsstatus,
								advertisstarttime,
								advertisendtime,
								advertistimeperiod,
								advertiscosts,
								modifydate,
								modifyuser,
								periodtimes
								)
							VALUES (
								@advertiseurl,
								@adsstatus,
								@advertisstarttime,
								@advertisendtime,
								@advertistimeperiod,
								@advertiscosts,
								now(),
								@modifyuser,
								@periodtimes
								)				
			       ");
            #endregion            
            parm.Add("@advertiseurl", request.advertiseurl);
            parm.Add("@adsstatus", Convert.ToInt32( request.adsstatus));
            parm.Add("@advertisstarttime", request.advertisstarttime);
            parm.Add("@advertisendtime", request.advertisendtime);
            parm.Add("@advertistimeperiod", request.advertistimeperiod);
            parm.Add("@advertiscosts", request.advertiscosts);
            parm.Add("@modifyuser", request.modifyuser);
            parm.Add("@periodtimes", request.periodtimes);

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

		#region 編輯廣告

		/// <summary>
		/// 編輯廣告
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult EditAdvertise(AdvertisesettingModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			string advertistimeperiod = request.advertistimeperiod;

			string[] arrays = advertistimeperiod.Split(',');
			request.periodtimes = arrays.Length;


			#region  sql
			querySql.Append(@"

						UPDATE PUBLIC.advertisesetting
						SET advertiseurl = @advertiseurl,
							adsstatus = @adsstatus,
							advertisstarttime = @advertisstarttime,
							advertisendtime = @advertisendtime,
							advertistimeperiod = @advertistimeperiod,
							advertiscosts = @advertiscosts,
							modifydate =now(),
							modifyuser = @modifyuser,
							periodtimes = @periodtimes
						WHERE advertiseid =@advertiseid

			       ");
			#endregion
			parm.Add("@advertiseurl", request.advertiseurl);
			parm.Add("@adsstatus", Convert.ToInt32(request.adsstatus));
			parm.Add("@advertisstarttime", request.advertisstarttime);
			parm.Add("@advertisendtime", request.advertisendtime);
			parm.Add("@advertistimeperiod", request.advertistimeperiod);
			parm.Add("@advertiscosts", request.advertiscosts);
			parm.Add("@modifyuser", request.modifyuser);
			parm.Add("@periodtimes", request.periodtimes);
			parm.Add("@advertiseid", request.advertiseid);

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

		#region 刪除廣告
		/// <summary>
		/// 刪除廣告
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult DeleteAdvertise(int advertiseid)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			var parm = new DynamicParameters();
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			         
					DELETE FROM public.advertisesetting
					WHERE advertiseid=@advertiseid
            ");

			var dt = new DataTable();
			parm.Add("@advertiseid", advertiseid);
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
