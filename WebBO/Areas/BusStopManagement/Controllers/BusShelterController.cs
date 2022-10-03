using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBO.Areas.BusStopManagement.Models;
using WebBO.General;
using WebBO.General.Repository.Connection;

namespace WebBO.Areas.BusStopManagement.Controllers
{
    public class BusShelterController
    {
        protected ConnectionFactory _connectionFactory;
        public BusShelterController()
        {
            _connectionFactory = new ConnectionFactory();

        }
		/// <summary>
		/// 取得智慧站牌
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult Getv_busshelter()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
			          SELECT id,
									district,
									busstopname,
									location,
									STATUS as statusName,
									villname,
									dir,
									suggestunit,
									types,
									undertaker,
									propertynum,
									setyears,
									registeryear::text,
									note,
									taipowernum,
									handlestat,
									movestat,
									stationnum
								FROM operationpolicy.v_busshelter
								WHERE STATUS = '已建置'
								ORDER BY id DESC;
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


		/// <summary>
		/// 新增候車亭
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult Createbusshelter(BusshelterModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			querySql.Append(@"
			  INSERT INTO operationpolicy.busshelter (
									district,
									busstopname,
									location,
									STATUS,
									suggestunit,
									types,
									undertaker,
									note,
									taipowernum,
									dir,
									handlestat,
									movestat,
									stationnum,
									villname,
									setyears,
									registeryear
									)
								VALUES (
									@district,
									@busstopname,
									@location,
									@STATUS,
									@suggestunit,
									@types,
									@undertaker,
									@note,
									@taipowernum,
									@dir,
									@handlestat,
									@movestat,
									@stationnum,
									@villname,
									@setyears,
									@registeryear
									);

            ");

			var dt = new DataTable();
			parm.Add("@district", request.district);
			parm.Add("@busstopname", request.busstopname);
			parm.Add("@location", request.location);
			parm.Add("@STATUS", 0);
			parm.Add("@suggestunit", request.suggestunit);
			parm.Add("@types", request.types);
			parm.Add("@undertaker", request.undertaker);
			parm.Add("@note", request.note);
			parm.Add("@taipowernum", request.taipowernum);
			parm.Add("@dir", request.dir);
			parm.Add("@handlestat", request.handlestat);
			parm.Add("@movestat", request.movestat);
			parm.Add("@stationnum", request.stationnum);
			parm.Add("@villname", request.villname);
			parm.Add("@setyears", request.setyears);
			parm.Add("@registeryear", request.registeryear);

			//dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}

		/// <summary>
		/// 編輯候車亭
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult Editbusshelter(BusshelterModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			querySql.Append(@"
					UPDATE operationpolicy.busshelter
					SET propertynum = @propertynum,
						registeryear = @registeryear,
						setyears = @setyears,
						handlestat = @handlestat,
						movestat = @movestat,
						STATUS = @status,
						note = @note
					WHERE id = @id;		
            ");

			var dt = new DataTable();
			parm.Add("@propertynum", request.propertynum);
			parm.Add("@registeryear", request.registeryear);
			parm.Add("@setyears", request.setyears);
			parm.Add("@handlestat", request.handlestat);
			parm.Add("@movestat", request.movestat);
			parm.Add("@status", request.status=="已建置"?1:0);
			parm.Add("@note", request.note);
			parm.Add("@id", request.id);
			//dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}

		/// <summary>
		/// 取得行政區下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetTownship()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
			          SELECT tnd_townshipno,
									tnd_townshipname,
									tnd_areacode,
									tnd_zipcode,
									tnd_longitude,
									tnd_latitude,
									tnd_usestatus,
									tnd_cityno
								FROM operationpolicy.townshipd
								WHERE tnd_cityno = 9
									AND RIGHT(tnd_townshipname, 1) = '區';
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
		/// <summary>
		/// 取得行政里下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetVillage(VillageModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
			        SELECT villname
								FROM village_moi
								WHERE townname IN (
										SELECT tnd_townshipname
										FROM operationpolicy.townshipd
										WHERE tnd_cityno = 9
											AND tnd_townshipno = @dis
											AND tnd_usestatus = 1
										)
									AND countyname = '臺中市'

            ");

			var dt = new DataTable();
			var parm = new DynamicParameters();
			parm.Add("@dis", request.dis);
			dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}

		/// <summary>
		/// 取得綁定站位下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetStation(VillageModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
			       SELECT   bsm_busstopno,
									bsm_chinesename || '-(' || (
										SELECT pad_parameterdescription
										FROM operationpolicy.parameterd
										WHERE pad_parametercategoryno = 2
											AND pad_parametervalue = bsm_streameddirection
										) || ')' bsm_chinesename
								FROM operationpolicy.busstopmnewest
								WHERE bsm_townshipno = @dis
									AND bsm_busstopno IS NOT NULL
								ORDER BY 1;
            ");

			var dt = new DataTable();
			var parm = new DynamicParameters();
			parm.Add("@dis", request.dis);
			dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}

		/// <summary>
		/// 取得會勘承辦人
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetUndertaker()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
			       SELECT    acm_logonaccount,
									acm_username
								FROM operationpolicy.accountm
								WHERE acm_rolenew IS NOT NULL
									AND acm_username NOT LIKE '%停用%'
									AND acm_username NOT LIKE '%離職%'
									AND acm_username NOT LIKE '%尚未%'
									AND acm_username != 'X'
									AND acm_rolenew = '4'
								ORDER BY 2;

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

		public class VillageModel
        {
			public int dis { get; set; }
		}

	}
}
