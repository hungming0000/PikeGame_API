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
    public class NewssettingController
    {
        protected ConnectionFactory _connectionFactory;
        public NewssettingController()
        {
            _connectionFactory = new ConnectionFactory();
        }
		#region 取得最新消息列表
		/// <summary>
		/// 取得最新消息列表
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetNewssetting()
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;

			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"		         
					SELECT * FROM public.newssetting
						ORDER BY newsid ASC 
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


		#region 取得最新消息byid
		/// <summary>
		/// 取得最新消息byid
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult GetNewssettingById(int newsid)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			var parm = new DynamicParameters();
			StringBuilder querySql = new StringBuilder();
			querySql.Append(@"			         
					SELECT *
					FROM PUBLIC.newssetting
					WHERE newsid = @newsid
					ORDER BY newsid ASC
            ");

			var dt = new DataTable();
			parm.Add("@newsid", newsid);
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

		#region 編輯最新消息

		/// <summary>
		/// 編輯最新消息
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult EditNews(NewssettingModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();	

			#region  sql
			querySql.Append(@"

						UPDATE PUBLIC.newssetting
								SET newsdescription = @newsdescription,
										modifydate = now(),
										modifyuser = @modifyuser
								WHERE newsid = @newsid


			       ");
			#endregion
			parm.Add("@newsdescription", request.newsdescription);
			parm.Add("@modifyuser", request.modifyuser);
			parm.Add("@newsid", request.newsid);			

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

		#region 新增最新消息

		/// <summary>
		/// 新增最新消息
		/// </summary>
		/// <returns></returns>
		public ExecuteCommandAPIResult CreateNews(NewssettingModel request)
		{
			IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
			string message = "新增成功";
			bool isSuccess = true;
			StringBuilder querySql = new StringBuilder();
			var parm = new DynamicParameters();

			#region  sql
			querySql.Append(@"
						INSERT INTO  public.newssetting (newsdescription,modifydate,modifyuser) 
						VALUES(
						@newsdescription,
						now(),
						@modifyuser
						)

			       ");
			#endregion
			parm.Add("@newsdescription", request.newsdescription);
			parm.Add("@modifyuser", request.modifyuser);		

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
