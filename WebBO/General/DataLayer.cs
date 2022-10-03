using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.ServiceModel;
using NLog;
using System.Timers;
using FCT;
//using FCT;

namespace WebBO
{
    /// <summary>
    /// DataLayer
    /// </summary>
    [Serializable]
    public class DBLayer : IDataLayer
    {
        #region 私有變數
        public static Logger WLogger = LogManager.GetLogger("DBLayer");
        protected static Dictionary<string, string> DicConnStr = new Dictionary<string, string>();
        protected static Assembly asm = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "\\" + GetConfig("ConnectionStringProvider"));
        private Timer timeout = new Timer();
        /// <summary>
        /// 物件回收
        /// </summary>       
        private bool disposed;
        private SqlConnection SqlConn = new SqlConnection();
        private SqlTransaction SqlTrans;

        #endregion
        /// <summary>
        /// 建構子
        /// </summary>
        public DBLayer()
        {
            this.CommandTimeout = 30;
            timeout.Interval = this.CommandTimeout * 1000;
            timeout.Elapsed += Timeout_Elapsed;
            WLogger.Debug("Connection dll CodeBase:{0}", asm.CodeBase);
            WLogger.Debug("Connection dll FullName:{0}", asm.FullName);
        }
        /// <summary>
        /// 存取連線字串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 存取指令執行逾時時間，預設為30秒
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// 取得連線狀態
        /// </summary>
        public bool IsConnected()
        {
            return SqlConn.State == ConnectionState.Open;
        }

        /// <summary>
        /// 設定指令類型
        /// </summary>
        public CommandType CommandType { set; get; }

        private void Timeout_Elapsed(object sender, ElapsedEventArgs e)
        {
            WLogger.Error("Transaction Timeout Rollback!");
            RollbackTrans();
        }

        /// <summary>
        /// 解構
        /// </summary>
        ~DBLayer()
        {
            Dispose(false);
        }
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
                disposed = true;
            }
        }
        protected static string GetConfig(string Key)
        {
            return ConfigurationManager.AppSettings[Key];
        }
        /// <summary>
        /// 設定連線字串
        /// </summary>
        /// <param name="ConnName">連線名稱</param>
        /// <returns></returns>
        public void SetConnectName(string ConnName)
        {
            try
            {
                WLogger.Debug("SetConnectName：{0}", ConnName);
                if (DicConnStr.ContainsKey(ConnName))
                {
                    SqlConn.ConnectionString = this.ConnectionString = DicConnStr[ConnName];
                }
                else
                {
                    WLogger.Debug("GetConnection：{0}", ConnName);
                    IConnectStringProvider isp = asm.CreateInstance("FCT.ConnStringProvider") as IConnectStringProvider;
                    object rs = isp.GetConnData(GetConfig(ConnName));
                    if (rs.GetType().Name == "String")
                    {
                        SqlConn.ConnectionString = this.ConnectionString = DicConnStr[ConnName] = rs.ToString();
                    }
                    if (rs.GetType().Name == "SqlConnection")
                        SqlConn = rs as SqlConnection;
                }
            }
            catch (Exception Ex)
            {
                WLogger.Error("SetConnectName Exception：{0}", Ex.Message);
                throw new FaultException(Ex.Message, new FaultCode("DB"));
            }
        }

        public void SetCommandType(string CmdType)
        {
            this.CommandType = (CmdType.ToLower() == "text") ? CommandType.Text : CommandType.StoredProcedure;
        }

        public void SetCommandTimeout(int Expire)
        {
            this.CommandTimeout = Expire;
            timeout.Interval = Expire * 1000;
        }
        /// <summary>
        /// 開啟資料庫連線
        /// </summary>
        public void Open()
        {
            try
            {
                if (SqlConn.State != ConnectionState.Open)
                {
                    SqlConn.Open();
                }
            }
            catch (Exception Ex)
            {
                WLogger.Error("Open Exception：{0}", Ex.Message);
                this.Close();
                throw new FaultException(Ex.Message, new FaultCode("DB"));
            }
        }
        /// <summary>
        /// 關閉資料庫連線
        /// </summary>
        public void Close()
        {
            if (SqlConn.State != ConnectionState.Closed)
            {
                SqlConn.Close();
            }
        }
        /// <summary>
        /// 開始交易
        /// </summary>
        public void BeginTrans()
        {
            FaultException xEx = null;
            try
            {
                if (SqlTrans == null)
                {
                    this.Open();
                    SqlTrans = SqlConn.BeginTransaction();
                    timeout.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                WLogger.Error("Begin Trans Exception：{0}", Ex.Message);
                xEx = new FaultException(Ex.Message, new FaultCode("DB"));
                SqlTrans = null;
                timeout.Enabled = false;
                this.Close();
            }
            if (xEx != null) throw xEx;
        }
        /// <summary>
        /// 完成交易
        /// </summary>
        public void CommitTrans()
        {
            FaultException xEx = null;
            if (SqlTrans != null)
            {
                try
                {
                    SqlTrans.Commit();
                }
                catch (Exception Ex)
                {
                    WLogger.Error("Commit Trans Exception：{0} ", Ex.Message);
                    xEx = new FaultException(Ex.Message, new FaultCode("DB"));
                    this.Close();
                }
                finally
                {
                    timeout.Enabled = false;
                    SqlTrans.Dispose();
                    SqlTrans = null;
                }
                if (xEx != null) throw xEx;
            }
        }
        /// <summary>
        /// 回復交易
        /// </summary>
        public void RollbackTrans()
        {
            if (SqlTrans != null)
            {
                FaultException xEx = null;
                try
                {
                    SqlTrans.Rollback();
                }
                catch (Exception Ex)
                {
                    WLogger.Error("Rollback Trans Exception：{0}", Ex.Message);
                    xEx = new FaultException(Ex.Message, new FaultCode("DB"));
                    this.Close();
                }
                finally
                {
                    timeout.Enabled = false;
                    SqlTrans.Dispose();
                    SqlTrans = null;
                }
                if (xEx != null) throw xEx;
            }
        }

        /// <summary>
        /// 取得執行指令結果的 DataTable 
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string SQLStr)
        {
            SqlDataAdapter SqlAdt = null;
            SqlCommand SqlCmd = null;
            DataTable dt = new DataTable();
            FaultException xEx = null;
            try
            {
                if (SqlTrans == null) this.Open();
                SqlCmd = SqlConn.CreateCommand();
                //SqlCmd.CommandTimeout = this.CommandTimeout;
                SqlCmd.CommandTimeout = 300;
                SqlCmd.CommandType = this.CommandType;
                SqlCmd.CommandText = SQLStr;
                SqlAdt = new SqlDataAdapter { SelectCommand = SqlCmd };
                SqlAdt.Fill(dt);
                dt.TableName = "t1";
            }
            catch (Exception Ex)
            {
                WLogger.Error("GetDataTable Exception：{0}", Ex.Message);
                dt.Dispose();
                xEx = new FaultException(Ex.Message, new FaultCode("DB"));
            }
            finally
            {
                if (SqlAdt != null)
                {
                    SqlAdt.Dispose();
                    SqlAdt = null;
                }
                if (SqlCmd != null)
                {
                    SqlCmd.Dispose();
                    SqlCmd = null;
                }
                if (SqlTrans == null) this.Close();
            }
            if (xEx != null) throw xEx;
            return dt;
        }
        /// <summary>
        /// 取得執行指令結果的 DataTable
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <param name="Parameters">指令參數集合</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string SQLStr, Dictionary<string, DBParamter> Parameters)
        {
            SqlDataAdapter SqlAdt = null;
            SqlCommand SqlCmd = null;

            DataTable dt = new DataTable();
            FaultException xEx = null;
            try
            {
                if (SqlTrans == null) this.Open();
                SqlCmd = SqlConn.CreateCommand();
                SqlCmd.CommandTimeout = this.CommandTimeout;
                SqlCmd.CommandType = this.CommandType;
                SqlCmd.CommandText = SQLStr;
                foreach (KeyValuePair<string, DBParamter> kvp in Parameters)
                {
                    SqlParameter dp = SqlCmd.CreateParameter();
                    dp.ParameterName = kvp.Key;
                    if (kvp.Value.IsBytes)
                        dp.Value = kvp.Value.Bytes;
                    else
                        dp.Value = kvp.Value.Value;
                    SqlCmd.Parameters.Add(dp);
                    dp = null;
                }
                SqlAdt = new SqlDataAdapter { SelectCommand = SqlCmd };
                SqlAdt.Fill(dt);
                dt.TableName = "t1";
            }
            catch (Exception Ex)
            {
                WLogger.Error("GetDataTable With Paramter Exception：{0}", Ex.Message);
                dt.Dispose();
                xEx = new FaultException(Ex.Message, new FaultCode("DB"));
            }
            finally
            {
                if (SqlAdt != null)
                {
                    SqlAdt.Dispose();
                    SqlAdt = null;
                }
                if (SqlCmd != null)
                {
                    SqlCmd.Dispose();
                    SqlCmd = null;
                }
                if (SqlTrans == null) this.Close();
            }
            if (xEx != null) throw xEx;
            return dt;
        }
        /// <summary>
        /// 執行查詢，並傳回查詢所傳回的結果集第一個資料列的第一個資料行。會忽略其他的資料行或資料列。
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <returns>DataTable</returns>
        public string ExecuteScalar(string SQLStr)
        {
            string Result = "";
            SqlCommand SqlCmd = null;
            FaultException xEx = null;
            try
            {
                if (SqlTrans == null) this.Open();
                SqlCmd = SqlConn.CreateCommand();
                SqlCmd.CommandType = this.CommandType;
                SqlCmd.CommandTimeout = this.CommandTimeout;
                SqlCmd.CommandText = SQLStr;
                if (SqlTrans != null)
                {
                    SqlCmd.Transaction = SqlTrans;
                }
                object objResult = SqlCmd.ExecuteScalar();
                Result = objResult!=null?objResult.ToString() : "";
                //Result = objResult?.ToString() ?? "";
            }
            catch (Exception Ex)
            {
                WLogger.Error("ExecuteScalar Exception：{0}", Ex.Message);
                xEx = new FaultException(Ex.Message, new FaultCode("DB"));
            }
            finally
            {
                if (SqlCmd != null)
                {
                    SqlCmd.Dispose();
                    SqlCmd = null;
                }
                if (SqlTrans == null) this.Close();
            }
            if (xEx != null) throw xEx;
            return Result;
        }
        /// <summary>
        /// 執行查詢，並傳回查詢所傳回的結果集第一個資料列的第一個資料行。會忽略其他的資料行或資料列。
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <param name="Parameters">指令參數集合</param>
        /// <returns>DataTable</returns>
        public string ExecuteScalar(string SQLStr, Dictionary<string, DBParamter> Parameters)
        {
            string Result = "";
            SqlCommand SqlCmd = null;
            FaultException xEx = null;
            try
            {
                if (SqlTrans == null) this.Open();
                SqlCmd = SqlConn.CreateCommand();
                SqlCmd.CommandType = this.CommandType;
                SqlCmd.CommandTimeout = this.CommandTimeout;
                SqlCmd.CommandText = SQLStr;
                if (SqlTrans != null)
                {
                    SqlCmd.Transaction = SqlTrans;
                }
                foreach (KeyValuePair<string, DBParamter> kvp in Parameters)
                {
                    SqlParameter dp = SqlCmd.CreateParameter();
                    dp.ParameterName = kvp.Key;
                    if (kvp.Value.IsBytes)
                        dp.Value = kvp.Value.Bytes;
                    else
                        dp.Value = kvp.Value.Value;
                    SqlCmd.Parameters.Add(dp);
                    dp = null;
                }
                object objResult = SqlCmd.ExecuteScalar();
                Result = objResult != null ? objResult.ToString() : "";
            }
            catch (Exception Ex)
            {
                WLogger.Error("ExecuteScalar With Paramter Exception：{0}", Ex.Message);
                xEx = new FaultException(Ex.Message, new FaultCode("DB"));
            }
            finally
            {
                if (SqlCmd != null)
                {
                    SqlCmd.Dispose();
                    SqlCmd = null;
                }
                if (SqlTrans == null) this.Close();
            }
            if (xEx != null) throw xEx;
            return Result;
        }
        /// <summary>
        /// 執行查詢，並傳回受影響的資料列數目
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <returns>受影像資料列數目</returns>
        public int ExecuteNonQuery(string SQLStr)
        {
            int Result = 0;
            SqlCommand SqlCmd = null;
            FaultException xEx = null;
            try
            {
                if (SqlTrans == null) this.Open();
                SqlCmd = SqlConn.CreateCommand();
                SqlCmd.CommandType = this.CommandType;
                SqlCmd.CommandTimeout = this.CommandTimeout;
                SqlCmd.CommandText = SQLStr;
                if (SqlTrans != null)
                {
                    SqlCmd.Transaction = SqlTrans;
                }
                Result = SqlCmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                WLogger.Error("ExecuteNonQuery Exception：{0}", Ex.Message);
                xEx = new FaultException(Ex.Message, new FaultCode("DB"));
            }
            finally
            {
                if (SqlCmd != null)
                {
                    SqlCmd.Dispose();
                    SqlCmd = null;
                }
                if (SqlTrans == null) this.Close();
            }
            if (xEx != null) throw xEx;
            return Result;
        }
        /// <summary>
        /// 執行查詢，並傳回受影響的資料列數目
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <param name="Parameters">指令參數集合</param>
        /// <returns>受影像資料列數目</returns>
        public int ExecuteNonQuery(string SQLStr, Dictionary<string, DBParamter> Parameters)
        {
            int Result = 0;
            SqlCommand SqlCmd = null;
            FaultException xEx = null;
            try
            {
                if (SqlTrans == null) this.Open();
                SqlCmd = SqlConn.CreateCommand();
                SqlCmd.CommandType = this.CommandType;
                SqlCmd.CommandTimeout = this.CommandTimeout;
                SqlCmd.CommandText = SQLStr;
                if (SqlTrans != null)
                {
                    SqlCmd.Transaction = SqlTrans;
                }
                foreach (KeyValuePair<string, DBParamter> kvp in Parameters)
                {
                    SqlParameter dp = SqlCmd.CreateParameter();
                    dp.ParameterName = kvp.Key;
                    if (kvp.Value.IsBytes)
                        dp.Value = kvp.Value.Bytes;
                    else
                        dp.Value = kvp.Value.Value;
                    SqlCmd.Parameters.Add(dp);
                    dp = null;
                }
                SqlCmd.CommandTimeout = this.CommandTimeout;
                Result = SqlCmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                WLogger.Error("ExecuteNonQuery With Paramter Exception：{0}", Ex.Message);
                xEx = new FaultException(Ex.Message, new FaultCode("DB"));
            }
            finally
            {
                if (SqlCmd != null)
                {
                    SqlCmd.Dispose();
                    SqlCmd = null;
                }
                if (SqlTrans == null) this.Close();
            }
            if (xEx != null) throw xEx;
            return Result;
        }

        protected object CheckValue(DBParamter DP)
        {
            object Result = DBNull.Value;
            if (DP.IsBytes)
            {
                if (DP.Bytes != null)
                    Result = DP.Bytes;
            }
            else
            {
                if (DP.Value != null)
                    Result = DP.Value;
            }
            return Result;
        }

        /// <summary>
        /// 終結託管資源
        /// </summary>
        public void Dispose()
        {
            try
            {

                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception Ex)
            {
                WLogger.Error("Dispose Exception：{0}", Ex.Message);
            }
        }
    }
}
