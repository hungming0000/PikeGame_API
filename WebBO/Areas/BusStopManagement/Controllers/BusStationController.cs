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
    public class BusStationController
    {
        protected ConnectionFactory _connectionFactory;
        public BusStationController()
        {
            _connectionFactory = new ConnectionFactory();

        }
		#region 取得停車場站
		/// <summary>
		/// 取得停車場站
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetBusstationm()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"
			       SELECT bnm_busstationno,
								bnm_stationname,
								bnm_longitude,
								bnm_latitude,
								bnm_stationusage,
								(
									SELECT pad_parameterdescription stationusage
									FROM operationpolicy.parameterd
									WHERE pad_parametervalue = bnm_stationusage
										AND pad_parametercategoryno = (
											SELECT pam_parametercategoryno
											FROM operationpolicy.parameterm
											WHERE pam_parametercategoryname = 'StationUsage'
											)
									),
								bnm_stationsuperior,
								bnm_cityno,
								(
									SELECT cym_cityname city
									FROM operationpolicy.citym
									WHERE cym_cityno = bnm_cityno
									),
								bnm_address,
								bnm_phone,
								bnm_fax,
								bnm_approvedspace,
								bnm_actualspace,
								bnm_planlargebus,
								bnm_planmediumbus,
								bnm_actuallargebus,
								bnm_actualmediumbus,
								bnm_stationownership,
								(
									SELECT pad_parameterdescription stationownership
									FROM operationpolicy.parameterd
									WHERE pad_parametervalue = bnm_stationownership
										AND pad_parametercategoryno = (
											SELECT pam_parametercategoryno
											FROM operationpolicy.parameterm
											WHERE pam_parametercategoryname = 'StationAcquireFrom'
											)
									) stationownership,
								bnm_footnote,
								bnm_usestatus,
								CASE bnm_usestatus
									WHEN 0
										THEN '未使用'
									WHEN 1
										THEN '使用中'
									WHEN 2
										THEN '取消'
									END STATUS,
								bnm_stationplaceno,
								bnm_startusedate,
								bnm_endusedate,
								bnm_acceptpark,
								bnm_documentissuedate,
								bnm_organizationno,
								(
									SELECT pad_parameterdescription organizationno
									FROM operationpolicy.parameterd
									WHERE pad_parametervalue = bnm_organizationno
										AND pad_parametercategoryno = (
											SELECT pam_parametercategoryno
											FROM operationpolicy.parameterm
											WHERE pam_parametercategoryname = 'StationOwnership'
											)
									) organizationno,
								bnm_documentissueno,
								bnm_townshipno,
								(
									SELECT tnd_townshipname
									FROM operationpolicy.townshipd
									WHERE tnd_townshipno = bnm_townshipno
									),
								bnm_operatecompanyno,
								(
									SELECT bcm_chinesename company
									FROM operationpolicy.buscompanym
									WHERE bcm_buscompanyno = bnm_operatecompanyno
									),
									(
									SELECT bcm_buscompanyno companyno
									FROM operationpolicy.buscompanym
									WHERE bcm_buscompanyno = bnm_operatecompanyno
									)
							FROM operationpolicy.busstationm
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
		public ExecuteCommandAPIResult EditBusstationm(BusstationmModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();
			querySql.Append(@"
					UPDATE operationpolicy.busstationm
					SET bnm_stationname = @bnm_stationname,
						bnm_longitude = @bnm_longitude,
						bnm_latitude = @bnm_latitude,
						bnm_stationusage = @bnm_stationusage,
						bnm_stationsuperior = @bnm_stationsuperior,
						bnm_cityno = @bnm_cityno,
						bnm_address = @bnm_address,
						bnm_phone = @bnm_phone,
						bnm_fax = @bnm_fax,
						bnm_approvedspace = @bnm_approvedspace,
						bnm_actualspace = @bnm_actualspace,
						bnm_planlargebus = @bnm_planlargebus,
						bnm_planmediumbus = @bnm_planmediumbus,
						bnm_actuallargebus = @bnm_actuallargebus,
						bnm_actualmediumbus = @bnm_actualmediumbus,
						bnm_stationownership = @bnm_stationownership,
						bnm_footnote = @bnm_footnote,
						bnm_usestatus = @bnm_usestatus,
						bnm_stationplaceno = @bnm_stationplaceno,
						bnm_startusedate = @bnm_startusedate,
						bnm_endusedate = @bnm_endusedate,
						bnm_acceptpark = @bnm_acceptpark,
						bnm_documentissuedate = @bnm_documentissuedate,
						bnm_organizationno = @bnm_organizationno,
						bnm_documentissueno = @bnm_documentissueno,
						bnm_townshipno = @bnm_townshipno,
						bnm_operatecompanyno = @bnm_operatecompanyno
					WHERE bnm_busstationno = @bnm_busstationno

            ");

			var dt = new DataTable();
			parm.Add("@bnm_stationname", request.bnm_stationname);
			parm.Add("@bnm_longitude", request.bnm_longitude);
			parm.Add("@bnm_latitude", request.bnm_latitude);
			parm.Add("@bnm_stationusage", request.bnm_stationusage);
			parm.Add("@bnm_stationsuperior", request.bnm_stationsuperior);
			parm.Add("@bnm_cityno", request.bnm_cityno );
			parm.Add("@bnm_address", request.bnm_address);
			parm.Add("@bnm_phone", request.bnm_phone);
			parm.Add("@bnm_fax", request.bnm_fax);
			parm.Add("@bnm_approvedspace", request.bnm_approvedspace);
			parm.Add("@bnm_actualspace", request.bnm_actualspace);
			parm.Add("@bnm_planlargebus", request.bnm_planlargebus);
			parm.Add("@bnm_planmediumbus", request.bnm_planmediumbus);
			parm.Add("@bnm_actuallargebus", request.bnm_actuallargebus);
			parm.Add("@bnm_actualmediumbus", request.bnm_actualmediumbus);
			parm.Add("@bnm_stationownership", request.bnm_stationownership);
			parm.Add("@bnm_footnote", request.bnm_footnote);
			parm.Add("@bnm_usestatus", request.bnm_usestatus);
			parm.Add("@bnm_stationplaceno", request.bnm_stationplaceno);
			parm.Add("@bnm_startusedate", request.bnm_startusedate);
			parm.Add("@bnm_endusedate", request.bnm_endusedate);
			parm.Add("@bnm_acceptpark", request.bnm_acceptpark);
			parm.Add("@bnm_documentissuedate", request.bnm_documentissuedate);
			parm.Add("@bnm_organizationno", request.bnm_organizationno);
			parm.Add("@bnm_documentissueno", request.bnm_documentissueno);
			parm.Add("@bnm_townshipno", request.bnm_townshipno);
			parm.Add("@bnm_operatecompanyno", request.bnm_operatecompanyno);
			parm.Add("@bnm_busstationno", request.bnm_busstationno);

			//dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

			return new ExecuteCommandAPIResult()
			{
				isSuccess = isSuccess,
				Message = message,
				Data = dt,
				Count = dt.Rows.Count,
			};
		}




		#region 下拉選單

		#region 場站業者下拉選單
		/// <summary>
		/// 取得場站業者下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetCompany()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			 
				SELECT bcm_buscompanyno,
					bcm_chinesename
				FROM operationpolicy.buscompanym;
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
        #region 狀態下拉選單
        /// <summary>
        /// 取得使用狀態下拉選單
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetStatus()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			 
				SELECT  0 STATUS,
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

		#region 場站型態下拉選單
		/// <summary>
		/// 取得場站型態下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetStationusage()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			 
				SELECT  pad_parametervalue,
								pad_parameterdescription stationusage,
								pad_parametercategoryno
							FROM operationpolicy.parameterd
							INNER JOIN operationpolicy.parameterm ON pam_parametercategoryname = 'StationUsage'
								AND pam_parametercategoryno = pad_parametercategoryno;

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

		#region 場站權屬下拉選單
		/// <summary>
		/// 取得場站權屬下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetStationownership()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			 
				SELECT  pad_parametervalue,
								pad_parameterdescription stationownership,
								pad_parametercategoryno
							FROM operationpolicy.parameterd
							INNER JOIN operationpolicy.parameterm ON pam_parametercategoryname = 'StationAcquireFrom'
								AND pam_parametercategoryno = pad_parametercategoryno;

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

		#region 縣市位置下拉選單
		/// <summary>
		/// 取得縣市位置下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetCity()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			 			
					SELECT cym_cityno,
						cym_cityname
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

		#region 行政區下拉選單
		/// <summary>
		/// 取得行政區下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetTownshipno()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			 			
					SELECT   tnd_townshipno,
									 tnd_townshipname
								FROM operationpolicy.townshipd;
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

		#region 	核准單位下拉選單
		/// <summary>
		/// 取得核准單位下拉選單
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetOrganizationno()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			 			
					SELECT  pad_parametervalue,
									pad_parameterdescription organizationno,
									pad_parametercategoryno
								FROM operationpolicy.parameterd
								INNER JOIN operationpolicy.parameterm ON pam_parametercategoryname = 'StationOwnership'
									AND pam_parametercategoryno = pad_parametercategoryno;
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

		#endregion
	}
}
