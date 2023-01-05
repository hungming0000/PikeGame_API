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
    public class VideosettingController
    {
        protected ConnectionFactory _connectionFactory;
        public VideosettingController()
        {
            _connectionFactory = new ConnectionFactory();
        }
        #region 取得影片列表
        /// <summary>
        /// 取得影片列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetVideosetting()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;

            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"			         
			SELECT videoid,
							videourl,
							CASE videostatus
								WHEN 0
									THEN '下架'
								WHEN 1
									THEN '上架'
								END videostatus,
                            videotitle,
                            videodescription,
							modifydate,
							modifyuser
						FROM PUBLIC.videosetting
						ORDER BY videoid ASC

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

        #region 取得影片列表Byid
        /// <summary>
        /// 取得影片列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetVideosettingById(int videoid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            var parm = new DynamicParameters();

            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"			         
			SELECT * FROM public.videosetting			
			WHERE videoid=@videoid
			ORDER BY videoid ASC 
            ");

            var dt = new DataTable();
            parm.Add("@videoid", videoid);
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

        #region 取得上架影片
        /// <summary>
        /// 取得上架影片
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetPutonVideo()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;

            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"			         
			SELECT videoid,
							videourl,
							videostatus,
                            videotitle,
                            videodescription,
							modifydate,
							modifyuser
						FROM PUBLIC.videosetting
						WHERE videostatus=1
						ORDER BY videoid ASC

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

        #region 新增影片

        /// <summary>
        /// 新增影片
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult CreateVideo(VideosesettingModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();
            var vstatus = Convert.ToInt32(request.videostatus);
            #region  sql
            querySql.Append(@"

							INSERT INTO PUBLIC.videosetting (
								videourl,
								videostatus,
                                videotitle,
                                videodescription,
								modifydate,
								modifyuser							
								)
							VALUES (
								@videourl,
								@videostatus,
                                @videotitle,
                                @videodescription,
								now(),
								@modifyuser							
								);

						
			       ");
            #endregion
            parm.Add("@videourl", request.videourl);
            parm.Add("@videostatus", Convert.ToInt32(request.videostatus));
            parm.Add("@videotitle", request.videotitle);
            parm.Add("@videodescription", request.videodescription);
            parm.Add("@modifyuser", request.modifyuser);

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

        #region 編輯影片

        /// <summary>
        /// 編輯廣告
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult EditVideo(VideosesettingModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();
            var vstatus = Convert.ToInt32(request.videostatus);

            #region  sql
            querySql.Append(@"
					UPDATE PUBLIC.videosetting
					SET 
						videourl = @videourl,
						videostatus = @videostatus,
						videotitle = @videotitle,
						videodescription = @videodescription,
						modifydate = now(),
						modifyuser = @modifyuser
					WHERE videoid = @videoid;					

			       ");
            #endregion
            parm.Add("@videourl", request.videourl);
            parm.Add("@modifyuser", request.modifyuser);
            parm.Add("@videostatus", vstatus);
            parm.Add("@videotitle", request.videotitle);
            parm.Add("@videodescription", request.videodescription);
            parm.Add("@videoid", request.videoid);


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

        #region 刪除影片
        /// <summary>
        /// 刪除影片
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult DeleteVideo(int videoid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            var parm = new DynamicParameters();
            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"			         
					DELETE FROM public.videosetting
					WHERE videoid=@videoid
            ");

            var dt = new DataTable();
            parm.Add("@videoid", videoid);
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
