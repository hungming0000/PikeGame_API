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
    public class BusStopController
    {
        protected ConnectionFactory _connectionFactory;
        public BusStopController()
        {
            _connectionFactory = new ConnectionFactory();

        }

        /// <summary>
        /// 取得站點
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetBusstopmnewest()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
			
                   SELECT bsm_serialid,
									bsm_busstopno,
									bsm_chinesename,
									bsm_englishname,
									CASE bsm_stopstyle
										WHEN 0
											THEN '傳統'
										WHEN 1
											THEN '大五'
										WHEN 2
											THEN '小五'
										END AS bsm_stopstyle,
									bsm_style,
									bsm_address,
									CASE bsm_streameddirection
										WHEN 1
											THEN '東'
										WHEN 2
											THEN '西'
										WHEN 3
											THEN '南'
										WHEN 4
											THEN '北'
										END bsm_streameddirection,
									bsm_longitude,
									bsm_latitude,
									(
										SELECT tnd_townshipname
										FROM operationpolicy.townshipd
										WHERE tnd_cityno = 9
											AND tnd_townshipno = bsm_townshipno
										) bsm_townshipno,
									bsm_usestatus,
									CASE bsm_usestatus
										WHEN 0
											THEN '未使用'
										WHEN 1
											THEN '使用中'
										WHEN 2
											THEN '取消'
										END STATUS
								FROM operationpolicy.busstopmnewest
								WHERE bsm_busstopno IS NOT NULL
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

        #region 編輯站點(高雄案未來可能不用站牌型態)
        /// <summary>
        /// 編輯站點(高雄案未來可能不用站牌型態)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteCommandAPIResult EditBusstopmnewest(UpdateBusstopmnewestModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            var message = "";
            bool isSuccess = false;       

            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();

            try
            {

                querySql.Append(@"
                                    UPDATE operationpolicy.busstopmnewest
                                    SET bsm_style = @bsm_style
                                    WHERE bsm_busstopno = @bsm_busstopno
                                    ");

                parm.Add("@bsm_style", request.bsm_style);
                parm.Add("@bsm_busstopno", request.bsm_style);



                dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = ex.Message.ToString();
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
    }
}
