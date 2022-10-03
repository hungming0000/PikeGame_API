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
     public class CompanyController
    {
        protected ConnectionFactory _connectionFactory;
        public CompanyController()
        {
            _connectionFactory = new ConnectionFactory();

        }
		#region 取得業者資料
		/// <summary>
		/// 取得業者資料
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetBuscompanym()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
				SELECT  bcm_buscompanyno,
								bcm_chinesename,
								bcm_englishname,
								bcm_cityno,
								(
									SELECT cym_cityname city
									FROM operationpolicy.citym
									WHERE cym_cityno = bcm_cityno
									),
								bcm_address,
								bcm_phone,
								bcm_fax,
								bcm_longitude,
								bcm_latitude,
								--bcm_thegeom,
								bcm_website,
								bcm_usestatus STATUS,
								case bcm_usestatus when 0 then '未使用' when 1 then '使用中' when 2 then '取消' end STATUStxt,
								bcm_mail
							FROM operationpolicy.buscompanym
							ORDER BY 1;
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

		/// <summary>
		/// 編輯停車場站
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult EditBuscompanym(BuscompanymModel request )
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			querySql.Append(@"				
						UPDATE operationpolicy.buscompanym
						SET bcm_chinesename = @bcm_chinesename,
							bcm_englishname = @bcm_englishname,
							bcm_cityno = @bcm_cityno,
							bcm_address = @bcm_address,
							bcm_phone = @bcm_phone,
							bcm_fax = @bcm_fax,
							bcm_longitude = @bcm_longitude,
							bcm_latitude = @bcm_latitude,
							bcm_website = @bcm_website,
							bcm_usestatus = @status
						WHERE bcm_buscompanyno::text = @bcm_buscompanyno;
            ");

			var dt = new DataTable();
			parm.Add("@bcm_chinesename", request.bcm_chinesename);
			parm.Add("@bcm_englishname", request.bcm_englishname);
			parm.Add("@bcm_cityno", request.bcm_cityno);
			parm.Add("@bcm_address", request.bcm_address);
			parm.Add("@bcm_phone", request.bcm_phone);
			parm.Add("@bcm_fax", request.bcm_fax);
			parm.Add("@bcm_longitude", request.bcm_longitude);
			parm.Add("@bcm_latitude", request.bcm_latitude);
			parm.Add("@bcm_website", request.bcm_website);
			parm.Add("@status", request.status);
			parm.Add("@bcm_buscompanyno", request.bcm_buscompanyno);

			dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}

		#region 取得縣市位置
		/// <summary>
		/// 取得縣市位置
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetCitym()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
				SELECT cym_cityno bcm_cityno ,cym_cityname city
				FROM operationpolicy.citym
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

		#region 取得使用狀態
		/// <summary>
		/// 取得使用狀態
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetStatus()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
				SELECT 0 STATUS,
								'未使用' txt

							UNION

							SELECT 1 STATUS,
								'使用中' txt

							UNION

							SELECT 2 STATUS,
								'取消' txt
							ORDER BY STATUS;

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
