using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using isRock.Framework;
using isRock.Framework.WebAPI;
using WebBO.General;
using WebBO.Models;
using DataTables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebBO.DataClass;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Dapper;
using WebBO.General.Repository.Connection;

namespace WebBO.Controllers
{

    /// <summary>
    /// 純登入使用
    /// </summary>
    public class UserInfo
    {
        public UserInfo()
        {
            IsVerify = false;
            IsEnable = false;
        }
        public string UserID { set; get; }
        public string UserName { set; get; }
        public bool IsEnable { set; get; }
        public bool IsVerify { set; get; }
        public static UserInfo VerifyUserInfo(string UID, string Pwd)
        {
            UserInfo ui = new UserInfo();

            DBLayer dl = CommonLib.GetDataLayer();
            //DataTable dt = dl.GetDataTable($"SELECT * From tbUserData Where UserID='{UID}' and PWD='{Pwd}'");
            DataTable dt = dl.GetDataTable(string.Format("SELECT * From tbUserData Where UserID='{0}' ", UID));
            //DataTable dt = dl.GetDataTable($"SELECT * From StaffData Where Account='{UID}' and PWD='{Pwd}'");

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ui.UserID = UID;
                ui.UserName = dr["UserName"].ToString();
                ui.IsEnable = true;
                ui.IsVerify = true;
            }
            return ui;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AccountmController : BusinessLogicBase
    {
        DataTable table = new DataTable();
        protected ConnectionFactory _connectionFactory;
        public AccountmController()
        {
            _connectionFactory = new ConnectionFactory();

        }     

        #region 使用者登入
        public class LoginRequest
        {
            public string accountid { get; set; }
            public string accpassword { get; set; }
        }
        /// <summary>
        /// 防止SQL Injection所以改寫Parameter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteCommandAPIResult Login(LoginRequest request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            var message = "";
            bool isSuccess = false;
            var pw = "";
            var dt = new DataTable();

            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
           
            var password = Extension.StringEncrypt._3DESCrypto.TripleDesDecryptorCBC(request.accpassword);

            //pw = crY.ClassLibrary.SecuritySolution.Cryptography.Cryptography.Current.Encrypt(password);
            try
            {

                querySql.Append(@"
                                  SELECT * FROM public.accountm
                                                             Where accountid= @id 
                                                             and accpassword= @password
                                                             ORDER BY accountid ASC
                                    ");

                parm.Add("@id", request.accountid);
                parm.Add("@password", password);

                table.Load(cn.ExecuteReader(querySql.ToString(), parm));
                if (table.Rows.Count > 0)
                {                   
                        //pw = table.Rows[0]["accpassword"].ToString();
                        //table.Rows[0]["accpassword"] = crY.ClassLibrary.SecuritySolution.Cryptography.Cryptography.Current.Decrypt(pw);
                        dt = table;
                        isSuccess = true;
                        message = "成功。";                   
                }
                else
                {
                    isSuccess = false;
                    message = "帳號或密碼錯誤。";
                }


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
                Count = table.Rows.Count,
            };
        }
        #endregion


        // 帳號管理
        #region 帳號管理 列表 GetUserData
        public ExecuteCommandAPIResult GetUserData(LoginRequest request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;

            var id = crY.ClassLibrary.SecuritySolution.Cryptography.Cryptography.Current.Encrypt(request.accountid);

            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
            SELECT * FROM public.accountm
                                     Where accountid= @id And acm_password=@password
                                     ORDER BY accountid ASC
            ");

            var parm = new DynamicParameters();
            parm.Add("@id", id);
            parm.Add("@password", request.accpassword);

            var dt = new DataTable();
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));
           

            return new ExecuteCommandAPIResult()
            {

                isSuccess = isSuccess,
                Message = message,
                Data = dt,
                Count = dt.Rows.Count
            };
        }

        #endregion 

        #region 帳號管理 某一筆 GetUserDataByID
        public ExecuteCommandAPIResult GetUserDataByID(string userID)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            var pw = "";

            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
           SELECT * FROM public.accountm
                                     Where acm_logonaccount= @id
                                     ORDER BY acm_logonaccount ASC
            ");

            var parm = new DynamicParameters();
            parm.Add("@id", userID);

            var dt = new DataTable();
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            pw = dt.Rows[0]["acm_password"].ToString();
            dt.Rows[0]["acm_password"] = crY.ClassLibrary.SecuritySolution.Cryptography.Cryptography.Current.Decrypt(pw);

            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = dt,
                Count = dt.Rows.Count,
            };
        }
        #endregion


        public class ChangePassWord
        {
            public string Password { get; set; }
            public string UserID { get; set; }
        }

        #region 變更密碼 ChangePassword
        public ExecuteCommandAPIResult ChangePassword(ChangePassWord request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;

            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
                SELECT * FROM public.accountm
                                     Where acm_logonaccount= @id
                                     ORDER BY acm_logonaccount ASC
            ");

            var parm = new DynamicParameters();
            parm.Add("@id", request.UserID);

            var dt = new DataTable();
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            querySql.Clear();

            querySql.Append(@"UPDATE public.accountm
                                                    SET acm_password=@password
                                                    WHERE acm_logonaccount=@id;
                                                    ");
            parm.Add("@password", crY.ClassLibrary.SecuritySolution.Cryptography.Cryptography.Current.Encrypt(request.Password));

            cn.ExecuteReader(querySql.ToString(), parm);



            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = null,
                Count = 1,
            };
        }

        #endregion

        #region 變更密碼後登入
        public ExecuteCommandAPIResult LoginAgain(string userID)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;

            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
           SELECT * FROM public.accountm
                                     Where acm_logonaccount= @id
                                     ORDER BY acm_logonaccount ASC
            ");

            var parm = new DynamicParameters();
            parm.Add("@id", userID);

            var dt = new DataTable();
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            if (dt.Rows.Count == 0)
            {
                isSuccess = false;
                message = "系統錯誤。";
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


        #region 帳號管理

        public ExecuteCommandAPIResult GetAccountm()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"			         
					SELECT  accountid,
	                                accountname,
	                                accountgroupname,
	                                accountstyle
                                FROM PUBLIC.accountm
                                ORDER BY accountid ASC

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

        public ExecuteCommandAPIResult GetAccountmById(string accountid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            querySql.Append(@"			         
					SELECT  *
                                FROM PUBLIC.accountm
                                WHERE accountid=@accountid
                                ORDER BY accountid ASC

            ");

            var dt = new DataTable();
            parm.Add("@accountid", accountid);
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = dt,
                Count = dt.Rows.Count,
            };
        }

        #region 帳號管理 新增 Create
        public ExecuteCommandAPIResult CreateAccountm(AccountmModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            var message = "成功。";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();

            querySql.Append(@"	
			    SELECT *
                FROM PUBLIC.accountm
                where accountid=@accountid
                ORDER BY accountid ASC

            ");
            parm.Add("@accountid", request.accountid);
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            if (dt.Rows.Count == 0)
            {
                querySql.Clear();
                querySql.Append(@"	
			    INSERT INTO PUBLIC.accountm
                VALUES (
	                @accountid,
	                @accountname,
	                @accpassword,
	                @accountgroupname,
	                @accountstyle
	                );
            ");
                parm.Add("@accountname", request.accountname);
                parm.Add("@accpassword", request.accpassword);
                parm.Add("@accountgroupname", request.accountgroupname);
                parm.Add("@accountstyle", request.accountstyle);

                cn.ExecuteReader(querySql.ToString(), parm);
            }
            else
            {
                isSuccess = false;
                message = "帳號重複！";
            }


            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = null,
                Count = 1,
            };
        }
        #endregion

        #region 帳號管理 編輯 Edit
        public ExecuteCommandAPIResult EditAccountm(AccountmModel request)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            var message = "成功。";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            var dt = new DataTable();

            querySql.Append(@"	
			    UPDATE PUBLIC.accountm
                SET accountname = @accountname,
	                accpassword = @accpassword,
	                accountgroupname = @accountgroupname,
	                accountstyle = @accountstyle
                WHERE accountid = @accountid


            ");
            parm.Add("@accountid", request.accountid);
            parm.Add("@accountname", request.accountname);
            parm.Add("@accpassword", request.accpassword);
            parm.Add("@accountgroupname", request.accountgroupname);
            parm.Add("@accountstyle", request.accountstyle);
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));


            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = null,
                Count = 1,
            };
        }
        #endregion

        #region 帳號管理 刪除 Delete
        public ExecuteCommandAPIResult DeleteAccountmById(string accountid)
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            var parm = new DynamicParameters();
            querySql.Append(@"			         
					DELETE 
                                FROM PUBLIC.accountm
                                WHERE accountid=@accountid 
            ");

            var dt = new DataTable();
            parm.Add("@accountid", accountid);
            dt.Load(cn.ExecuteReader(querySql.ToString(), parm));

            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = null,
                Count = 1,
            };
        }
        #endregion


        #region 取得帳號類型
        /// <summary>
        /// 取得選手列表
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandAPIResult GetAccountgroupname()
        {
            IDbConnection cn = _connectionFactory.CreateConnection("Pgsql");
            string message = "";
            bool isSuccess = true;
            StringBuilder querySql = new StringBuilder();
            querySql.Append(@"
			    SELECT *
                FROM (
	                SELECT 0 AS accountstyle,
		                '比賽制定者' AS accountgroupname
	
	                UNION
	
	                SELECT 1 AS accountstyle,
		                '裁判' AS accountgroupname
	
	                UNION
	
	                SELECT 2 AS accountstyle,
		                '選手' AS accountgroupname
	
	                UNION
	
	                SELECT 3 AS accountstyle,
		                '觀眾' AS accountgroupname
	                ) a
                ORDER BY accountstyle
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

