using Dapper;
using isRock.Framework;
using System.Data;
using System.Text;
using WebBO.General;
using WebBO.General.Repository.Connection;

namespace WebBO.Controllers
{
    public class HmMenuController : BusinessLogicBase
    {
        protected ConnectionFactory _connectionFactory;
        public HmMenuController()
        {
            _connectionFactory = new ConnectionFactory();

        }

        #region 列表管理  GetHmMenu
        public ExecuteCommandAPIResult GetHmMenu()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;          

            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
                  SELECT 
                        hmm_id,
                        hmm_use,
                        hmm_pid, 
                        hmm_seq, 
                        hmm_imgurl, 
                        hmm_url, 
                        hmm_name, 
                        hmm_desc ,
                        CASE hmm_name when '站位'then 'BusStop' when '智慧型站牌' then 'IntelStop'
						when '候車亭' then 'BusShelter' when '候車亭建置計畫案件' then 'NewShlter'
						when '候車亭建置計畫案件' then 'NewShlter'
						when '停車場站' then 'BusStation'
						when '業者資料' then 'Company' end hmmrouter
                  FROM operationpolicy.hmmenu  
                  WHERE hmm_use=1 AND hmm_name<>'主頁模板' AND hmm_name<>'GIS行動版' 
                    AND hmm_name<>'營運月報管理' 
                    AND hmm_name<>'虧損補貼作業'
                    AND hmm_name<>'電子票證稽核'
                    AND hmm_name<>'自動稽核管理'
                  ORDER BY hmm_use desc,hmm_pid, hmm_seq     
            ");           
            //家瑋說營運月報到票證稽核、自動稽核管理先隱藏
            var dt = new DataTable();
            dt.Load(cn.ExecuteReader(querySql.ToString()));


            return new ExecuteCommandAPIResult()
            {

                isSuccess = isSuccess,
                Message = message,
                Data = dt,
                Count = dt.Rows.Count
            };
        }

        #endregion 

    }
}
