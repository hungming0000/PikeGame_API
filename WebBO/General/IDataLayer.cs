using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace WebBO
{

    /// <summary>
    /// 資料存取物件介面
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IDataLayer : IDisposable
    {
        /// <summary>
        /// 設定連線字串
        /// </summary>
        /// <param name="ConnName">連線名稱</param>
        /// <returns></returns>
        [OperationContract]
        void SetConnectName(string ConnName);
        /// <summary>
        /// 設定 CommandType Text or Sp(StoredProcedure)
        /// </summary>
        /// <param name="CmdType"></param>
        [OperationContract]
        void SetCommandType(string CmdType);
        /// <summary>
        /// 設定指令執行逾期時間，單位為秒
        /// </summary>
        /// <param name="Expire"></param>
        [OperationContract]
        void SetCommandTimeout(int Expire);
        /// <summary>
        /// 開啟資料庫連線
        /// </summary>
        [OperationContract]
        void Open();
        /// <summary>
        /// 關閉資料庫連線
        /// </summary>
        [OperationContract]
        void Close();
        /// <summary>
        /// 開始交易
        /// </summary>
        [OperationContract]
        void BeginTrans();
        /// <summary>
        /// 完成交易
        /// </summary>
        [OperationContract]
        void CommitTrans();
        /// <summary>
        /// 回復交易
        /// </summary>
        [OperationContract]
        void RollbackTrans();
        /// <summary>
        /// 取得執行指令結果的 DataTable 
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <returns>DataTable</returns>
        [OperationContract]
        DataTable GetDataTable(string SQLStr);
        /// <summary>
        /// 取得執行指令結果的 DataTable
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <param name="Parameters">指令參數集合</param>
        /// <returns>DataTable</returns>
        [OperationContract(Name = "GetDataTableWithParamter")]
        DataTable GetDataTable(string SQLStr, Dictionary<string, DBParamter> Parameters);
        /// <summary>
        /// 執行查詢，並傳回查詢所傳回的結果集第一個資料列的第一個資料行。會忽略其他的資料行或資料列。
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <returns>DataTable</returns>
        [OperationContract]
        string ExecuteScalar(string SQLStr);
        /// <summary>
        /// 執行查詢，並傳回查詢所傳回的結果集第一個資料列的第一個資料行。會忽略其他的資料行或資料列。
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <param name="Parameters">指令參數集合</param>
        /// <returns>DataTable</returns>
        [OperationContract(Name = "ExecuteScalarWithParamter")]
        string ExecuteScalar(string SQLStr, Dictionary<string, DBParamter> Parameters);
        /// <summary>
        /// 執行查詢，並傳回受影響的資料列數目
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <returns>受影像資料列數目</returns>
        [OperationContract]
        int ExecuteNonQuery(string SQLStr);
        /// <summary>
        /// 執行查詢，並傳回受影響的資料列數目
        /// </summary>
        /// <param name="SQLStr">執行指令</param>
        /// <param name="Parameters">指令參數集合</param>
        /// <returns>受影像資料列數目</returns>
        [OperationContract(Name = "ExecuteNonQueryWithParamter")]
        int ExecuteNonQuery(string SQLStr, Dictionary<string, DBParamter> Parameters);
    }

    [DataContract]
    public class DBParamter
    {
        public DBParamter()
        {
            Bytes = null;
            Value = null;
            IsBytes = false;
        }
        [DataMember]
        public byte[] Bytes;
        [DataMember]
        public string Value;
        [DataMember]
        public bool IsBytes;
    }
}
