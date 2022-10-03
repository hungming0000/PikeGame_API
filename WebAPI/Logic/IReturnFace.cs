using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using isRock.Framework;
using isRock.Framework.WebAPI;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using WebBO.General;

namespace WebAPI.Logic
{


    /// <summary>
    /// 自定義的回傳集合 可以回傳isSuccess,Message,Count,Data
    /// </summary>
    public class exeResult : ExecuteCommandDefaultResult
    {


        public object Hub { get; set; }


        public exeResult()
        {

        }
        public List<Dictionary<string, object>> GetResultData()
        {

            List<Dictionary<string, object>> _object = new List<Dictionary<string, object>>();
            Dictionary<string, object> dc_data = new Dictionary<string, object>();
            Dictionary<string, object> dc_cnt = new Dictionary<string, object>();
            Dictionary<string, object> dc_issuccess = new Dictionary<string, object>();
            Dictionary<string, object> dc_message = new Dictionary<string, object>();
            Dictionary<string, object> dc_hub = new Dictionary<string, object>();

            dc_issuccess.Add("isSuccess", this.isSuccess);
            dc_message.Add("Message", this.Message);
            dc_data.Add("Data", this.Data);
            if (this.Data != null)
            {
                dc_cnt.Add("Count", (Data as System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>).Count);
            }
            else
            {
                dc_cnt.Add("Count", null);
            }


            dc_hub.Add("Hub", this.Hub as Dictionary<string, object>);

            _object.Add(dc_issuccess);
            _object.Add(dc_message);
            _object.Add(dc_cnt);
            _object.Add(dc_data);
            _object.Add(dc_hub);


            return _object;
        }
    }


    /// <summary>
    /// 自定義的回傳集合
    /// </summary>
    public static class ReturnFace
    {
        /// <summary>
        /// 簡單處理型態問題
        /// </summary>
        /// <param name="oper">運算子＝、like </param>
        /// <param name="type">類型char or else</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string BuildValueByOperAndType(string oper, string type, string value)
        {
            string renVal = "";
            if (oper.ToLower() == "like")
            {
                if (type.ToLower().Contains("char"))
                {
                    renVal = "'%" + value + "%'";
                }
            }
            else
            {
                if (type.ToLower().Contains("char"))
                {
                    renVal = "'" + value + "'";
                }
                else
                {
                    renVal = value;
                }
            }
            return renVal;
        }

        /// <summary>
        /// 組成SQL語法
        /// </summary>
        /// <param name="TableName">Table名稱</param>
        /// <param name="obj">JSON 物件</param>
        /// <returns></returns>
        public static string ReturnSQL(string TableName, JObject obj)
        {
            StringBuilder sb = new StringBuilder();
            string type = obj["Type"].ToString();
            //string whereStr = "";

            string SQLStr = "";
            switch (type)
            {
                default:
                case "Select":
                    StringBuilder fieldsValues = new StringBuilder();
                    StringBuilder whereValues_S = new StringBuilder();
                    for (int idx = 0; idx <= obj["Fileds"].Count() - 1; idx++)
                    {

                        fieldsValues.AppendFormat(" {0} ,", obj["Fileds"][idx].ToString());

                    }
                    fieldsValues.Remove(fieldsValues.Length - 1, 1);

                    var w_S = obj["Where"];
                    //obj["Where"]["logic"]
                    for (int idx = 0; idx <= w_S["fields"].Count() - 1; idx++)
                    {
                        string strVal = BuildValueByOperAndType(w_S["operas"][idx].ToString(), w_S["types"][idx].ToString(), w_S["values"][idx].ToString());
                        whereValues_S.AppendFormat(" {0} {1} {2} and ", w_S["fields"][idx].ToString(), w_S["operas"][idx].ToString(), strVal);
                    }
                    whereValues_S.Remove(whereValues_S.Length - 4, 4);

                    SQLStr = string.Format(@" select  {1}  from {0} where {2}", TableName, fieldsValues.ToString(), whereValues_S.ToString());

                    break;


                case "Update":
                    StringBuilder updateValues = new StringBuilder();
                    StringBuilder outValues = new StringBuilder();
                    // string outStr = " output deleted.StaffNAME as Bef, inserted.StaffNAME as Aft";
                    for (int idx = 0; idx <= obj["Fileds"].Count() - 1; idx++)
                    {
                        string strVal = BuildValueByOperAndType(obj["Operas"][idx].ToString(), obj["Types"][idx].ToString(), obj["Values"][idx].ToString());
                        updateValues.AppendFormat(" {0} {1} {2} ,", obj["Fileds"][idx].ToString(), obj["Operas"][idx].ToString(), strVal);
                        outValues.AppendFormat(" deleted.{0} as {0}_Bef,inserted.{0} as {0}_Aft ,", obj["Fileds"][idx].ToString());
                    }
                    updateValues.Remove(updateValues.Length - 1, 1);
                    outValues.Remove(outValues.Length - 1, 1);
                    string outStr = " output " + outValues.ToString();

                    StringBuilder whereValues = new StringBuilder();
                    var w = obj["Where"];
                    //obj["Where"]["logic"]
                    for (int idx = 0; idx <= w["fields"].Count() - 1; idx++)
                    {
                        string strVal = BuildValueByOperAndType(w["operas"][idx].ToString(), w["types"][idx].ToString(), w["values"][idx].ToString());
                        whereValues.AppendFormat(" {0} {1} {2} and ", w["fields"][idx].ToString(), w["operas"][idx].ToString(), strVal);
                    }
                    whereValues.Remove(whereValues.Length - 4, 4);




                    SQLStr = string.Format(@" update {0}  set {1} {3} where {2}", TableName, updateValues.ToString(), whereValues.ToString(), outStr);

                    break;



                case "Insert":
                    StringBuilder fieldValues_I = new StringBuilder();
                    StringBuilder insertValues = new StringBuilder();
                    StringBuilder outValues_I = new StringBuilder();
                    // string outStr = " output deleted.StaffNAME as Bef, inserted.StaffNAME as Aft";
                    for (int idx = 0; idx <= obj["Fileds"].Count() - 1; idx++)
                    {
                        string strVal = BuildValueByOperAndType(obj["Operas"][idx].ToString(), obj["Types"][idx].ToString(), obj["Values"][idx].ToString());

                        fieldValues_I.AppendFormat(" {0} ,", obj["Fileds"][idx].ToString());
                        insertValues.AppendFormat(" {0} ,", strVal);

                        outValues_I.AppendFormat(" inserted.{0} as {0}_Aft ,", obj["Fileds"][idx].ToString());
                    }

                    //updateValues.AppendFormat(" {0} {1} {2} ,", obj["Fileds"][idx].ToString(), obj["Operas"][idx].ToString(), strVal);
                    fieldValues_I.Remove(fieldValues_I.Length - 1, 1);
                    insertValues.Remove(insertValues.Length - 1, 1);
                    outValues_I.Remove(outValues_I.Length - 1, 1);
                    string outStr_I = " output " + outValues_I.ToString();

                    //StringBuilder whereValues_I = new StringBuilder();
                    //var w_I = obj["Where"];
                    ////obj["Where"]["logic"]
                    //for (int idx = 0; idx <= w_I["fields"].Count() - 1; idx++)
                    //{
                    //    string strVal = BuildValueByOperAndType(w_I["operas"][idx].ToString(), w_I["types"][idx].ToString(), w_I["values"][idx].ToString());
                    //    whereValues_I.AppendFormat(" {0} {1} {2} and ", w_I["fields"][idx].ToString(), w_I["operas"][idx].ToString(), strVal);
                    //}
                    //whereValues_I.Remove(whereValues_I.Length - 4, 4);




                    SQLStr = string.Format(@" insert into {0}  ({1})  {3} Values({2})", TableName, fieldValues_I.ToString(), insertValues.ToString(), outStr_I);

                    break;

                case "Delete":

                    string outStr_D = " output deleted.* ";

                    StringBuilder whereValues_D = new StringBuilder();
                    var w_D = obj["Where"];
                    //obj["Where"]["logic"]
                    for (int idx = 0; idx <= w_D["fields"].Count() - 1; idx++)
                    {
                        string strVal = BuildValueByOperAndType(w_D["operas"][idx].ToString(), w_D["types"][idx].ToString(), w_D["values"][idx].ToString());
                        whereValues_D.AppendFormat(" {0} {1} {2} and ", w_D["fields"][idx].ToString(), w_D["operas"][idx].ToString(), strVal);
                    }
                    whereValues_D.Remove(whereValues_D.Length - 4, 4);




                    SQLStr = string.Format(@" delete  from {0}  {2} where {1}", TableName, whereValues_D.ToString(), outStr_D);

                    break;


            }




            return SQLStr;
        }
        public static ExecuteCommandDefaultResult<List<Dictionary<string, object>>> ReturnData(string sql)
        {
            {
                try
                {
                    WebBO.DBLayer dl = CommonLib.GetDataLayer();
                    DataTable dt = dl.GetDataTable(sql);
                    var dts = CommonLib.DataTableToListObject(dt);
                    return new ExecuteCommandDefaultResult<List<Dictionary<string, object>>>()
                    {
                        isSuccess = true,
                        Data = dts,
                        Message = "成功"
                    };

                }
                catch (Exception err)
                {
                    return new ExecuteCommandDefaultResult<List<Dictionary<string, object>>>()
                    {
                        isSuccess = false,
                        Data = null,
                        Message = err.Message
                    };

                }
            }
        }
    }

}