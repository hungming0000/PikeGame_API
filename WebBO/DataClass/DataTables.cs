// Turns the Ajax call parameters into a DataTableParameter object
// Permission to use this code for any purpose and without fee is hereby granted.
// No warrantles.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using WebBO.DataClass;
using WebBO.Models;
using WebBO.General;
using System.Data;

namespace DataTables
{
    public static class DataTableExtension
    {
        public enum operaType
        {
            _like,
            _not_like,
            _eqal,
            _not_eqal,
            _more_than,
            l_ess_than,
            _in,
            _notin,
        }
        public enum fieldType
        {
            _string,
            _int,
            _date,
            _bool,
        }

        public static string GetOrderByString(this DataTableParameters DP)
        {
            return string.Format(" ORDER BY {0} {1}  ", DP.Columns[(DP.Order.ToList()[0]).Value.Column].Name, (DP.Order.ToList()[0]).Value.Direction);
        }
        public static string GetWhereString(this DataTableSearchs DS, string fieldName, fieldType fieldType, operaType operaType)
        {
            StringBuilder sb = new StringBuilder();


            foreach (KeyValuePair<int, DataTableColumnSearch> DCS in DS.Columns)
            {
                if (DCS.Value.Name.Trim().ToLower() == fieldName.Trim().ToLower() || fieldName.Trim() == "")
                {
                    switch (operaType)
                    {
                        case operaType._like:
                            sb.AppendFormat(" and {0} like '%{1}%' ", DCS.Value.Name.Trim(), DCS.Value.Value.Trim());
                            break;
                        case operaType._eqal:

                            if (fieldType == fieldType._string)
                            {
                                sb.AppendFormat(" and {0} = '{1}' ", DCS.Value.Name.Trim(), DCS.Value.Value.Trim());
                            }
                            else
                            {
                                sb.AppendFormat(" and {0} = {1} ", DCS.Value.Name.Trim(), DCS.Value.Value.Trim());
                            }

                            break;


                    }
                }

                //  sb.AppendFormat( DCS.Value.Name)
            }

            return sb.ToString();
            //return string.Format(" {0} {1}  ", DP.Columns[(DP.Order.ToList()[0]).Value.Column].Name, (DP.Order.ToList()[0]).Value.Direction);

            //return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static string GetWhereString(this DataTableSearchs DS, List<SearchParams> searchParams, fieldType fieldType, operaType operaType)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in searchParams)
            {

                if (!string.IsNullOrEmpty(item.Value))
                {
                    // 以辦結案件 => 預設不帶歷史件
                    if (item.Name == "AppCloseResult")
                    {
                        if(item.Value == "0")
                        {
                            sb.Append($" and {item.Name.Trim()} is null ");
                        }
                        else
                        {
                            sb.Append($" and {item.Name.Trim()} = '{item.Value}' ");
                        }
                       
                    }
                    else
                    {
                        switch (operaType)
                        {
                            case operaType._like:
                                sb.Append($" and {item.Name.Trim()} like '%{item.Value}%' ");
                                break;
                            case operaType._eqal:
                                if (fieldType == fieldType._string)
                                {
                                    sb.Append($" and {item.Name.Trim()} = '{item.Value}' ");
                                }
                                else
                                {
                                    sb.Append($" and {item.Name.Trim()} = {item.Value} ");
                                }
                                break;
                        }
                    }



                    
                }

            }

            return sb.ToString();
        }
        public static string GetWhereStringByOT(this DataTableSearchs DS, List<SearchParams> searchParams)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in searchParams)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    switch (item.OperaType)
                    {
                        case "_like":
                            sb.Append($" and {item.Name.Trim()} like '%{item.Value}%' ");
                            break;
                        case "_eqal":
                            sb.Append($" and {item.Name.Trim()} = '{item.Value}' ");
                            //if (item.FieldType == "_string")
                            //{
                            //    sb.Append($" and {item.Name.Trim()} = '{item.Value}' ");
                            //}
                            //else
                            //{
                            //    sb.Append($" and {item.Name.Trim()} = {item.Value} ");
                            //}
                            break;
                    }
                }
            }

            return sb.ToString();
        }
        public static string GetWhereString(this DataTableSearchs DS, List<SearchParams> searchParams)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in searchParams)
            {

                if (item.Value == "")
                    continue;
                if (item.Name == "checkvsuser")
                {
                    sb.Append(string.Format($" and '{item.Value}' in (select SurveyUserID from V_SurveyAppType where AppContractTypeID=vsl.AppContractTypeID and AppSurveyStateName!='取消會勘')"));
                    continue;
                }
                if (!string.IsNullOrEmpty(item.Value))
                {
                    switch (item.OperaType)
                    {
                        case "like":
                            sb.Append($" and {item.Name.Trim()} like '%{item.Value}%' ");
                            break;
                        case "in":
                            if (item.Name == "SHUsers")
                                sb.Append($" and SurveyID in (select AppSurveyID from tbAppSurveyUser where UserID in ({item.Value}))");
                            else if (item.Name == "WS")
                                sb.Append($" and WorkStationID in (select WorkStationID from tbWorkStation where WorkStationName='{item.Value}')");
                            else
                                sb.Append($" and {item.Name.Trim()} in ({item.Value}) ");
                            break;
                        case "eqal":
                            if (item.FieldType == "string")
                            {
                                sb.Append($" and {item.Name.Trim()} = '{item.Value}' ");
                            }
                            else
                            {
                                sb.Append($" and {item.Name.Trim()} = {item.Value} ");
                            }
                            break;
                    }
                }
            }

            return sb.ToString();
        }

        public static string GetLandNameWhereString(this DataTableSearchs DS, List<SearchParams> searchParams)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in searchParams)
            {
                if (item.Name == "LandNameTemp")
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        var value = item.Value.Split(',');

                        foreach (var i in value)
                        {
                            var option = i.Split('_');

                            // 事業區
                            if (option[0] == "WA")
                            {
                                if (option.Count() == 2)
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append($" LongLandName like '%{option[1]}事業區%' ");
                                    }
                                    else
                                    {
                                        sb.Append($" or LongLandName like '%{option[1]}事業區%' ");
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append($" LongLandName like '%{option[1]}事業區{option[2]}%' ");
                                    }
                                    else
                                    {
                                        sb.Append($" or LongLandName like '%{option[1]}事業區{option[2]}%' ");
                                    }

                                }

                            }

                            // 縣市
                            if (option[0] == "CT")
                            {
                                if (option.Count() == 2)
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append($" LongLandName like '%{option[1]}%' ");
                                    }
                                    else
                                    {
                                        sb.Append($" or LongLandName like '%{option[1]}%' ");
                                    }
                                }
                                else
                                {
                                    if (sb.Length == 0)
                                    {
                                        sb.Append($" LongLandName like '%{option[1]}{option[2]}%' ");
                                    }
                                    else
                                    {
                                        sb.Append($" or LongLandName like '%{option[1]}{option[2]}%' ");
                                    }

                                }
                            }

                        }
                    }
                }
            }

            if (sb.Length == 0)
            {
                return sb.ToString();
            }
            else
            {
                return $" and ( {sb.ToString()} )";
            }

        }

        public static string CreateSqlString(this DataTableSearchs DS, string mainID, string tableName, string MainIDname, List<SearchParams> searchs, out string MID)
        {
            System.Data.DataTable dt = new DataTable();
            if (MainIDname != "")
                dt = WebBO.General.CommonLib.GetDataFromSQL(string.Format($@"select * from {tableName} where {MainIDname}='{mainID}'"));

            TSqlMaker sm = new TSqlMaker();
            sm.TableName = tableName;
            foreach (var item in searchs)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    switch (item.OperaType)
                    {
                        case "like":
                            //sb.Append($" and {item.Name.Trim()} like '%{item.Value}%' ");

                            break;
                        case "eqal":
                            switch (item.FieldType)
                            {
                                case "string":
                                    sm.AddFieldValue(item.Name.Trim(), item.Value, "'");
                                    break;
                                case "int":
                                    sm.AddFieldValue(item.Name.Trim(), item.Value, "");
                                    break;
                                case "date":
                                    DateTime GetD = new DateTime();
                                    if (item.Value.Split('/').Length == 1)
                                        item.Value = item.Value + "/01";
                                    if (item.Value.Split('/')[0].Length == 3)
                                        GetD = DateTime.Parse("0" + item.Value);
                                    else if (item.Value.Split('/')[0].Length == 2)
                                        GetD = DateTime.Parse("00" + item.Value);
                                    else if (item.Value.Split('/')[0].Length == 1)
                                        GetD = DateTime.Parse("000" + item.Value);
                                    else
                                        GetD = DateTime.Parse(item.Value);
                                    if (GetD.Year < 1911)
                                        GetD = GetD.AddYears(1911);
                                    sm.AddFieldValue(item.Name.Trim(), GetD.ToString("yyyy/MM/dd"), "'");
                                    break;
                                case "datetime":
                                    DateTime GetDT = DateTime.Parse(item.Value);
                                    if (GetDT.Year < 1911)
                                        GetDT = GetDT.AddYears(1911);
                                    sm.AddFieldValue(item.Name.Trim(), GetDT.ToString("yyyy/MM/dd HH:mm:ss"), "'");
                                    break;
                                default:
                                    break;
                            }
                            //if (item.FieldType == "string")
                            //{
                            //    //sb.Append($" and {item.Name.Trim()} = '{item.Value}' ");
                            //    sm.AddFieldValue(item.Name.Trim(), item.Value, "'");
                            //}
                            //else
                            //{
                            //    //sb.Append($" and {item.Name.Trim()} = {item.Value} ");
                            //    sm.AddFieldValue(item.Name.Trim(), item.Value, "");
                            //}
                            break;
                    }
                }
            }
            if (MainIDname != "")
            {
                if (dt.Rows.Count > 0)
                {
                    sm.AddWhereFieldValue(MainIDname, mainID, "'");
                    MID = mainID;
                    return sm.GenUpdate();
                }
                else
                {
                    mainID = Guid.NewGuid().ToString("N");
                    sm.AddFieldValue(MainIDname, mainID, "'");
                    MID = mainID;
                    return sm.GenInsert();
                }
            }
            else
            {
                MID = "";
                return sm.GenInsert();
            }
        }
        #region 查詢資料用 GetDataTable
        public static ExecuteCommandAPIResult GetDataTable(this DataTableSearchs DS, string SQLstr)
        {
            var message = "";
            bool isSuccess;

            DataTable dt = new DataTable();
            try
            {
                dt = CommonLib.GetDataFromSQL(SQLstr);

                if (dt.Rows.Count > 0)
                {
                    isSuccess = true;
                    message = "成功。";
                }
                else
                {
                    isSuccess = false;
                    message = "無資料。";
                }
            }
            catch (Exception e)
            {
                return new ExecuteCommandAPIResult()
                {
                    isSuccess = false,
                    Message = e.Message,
                    Data = null,
                    Count = 0,
                };
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


    public class DataTableColumnSearch
    {
        public int Data;
        public string Name;
        public string Value;


        private DataTableColumnSearch()
        {
        }

        /// <summary>
        /// Retrieve the DataTables Columns dictionary from a JSON parameter list
        /// </summary>
        /// <param name="input">JToken object</param>
        /// <returns>Dictionary of Column elements</returns>
        public static Dictionary<int, DataTableColumnSearch> Get(JToken input)
        {

            return (
                          (JArray)input)
                          .Select(col => new DataTableColumnSearch
                          {
                              Data = ((JArray)input).IndexOf(col),
                              Name = (string)col["name"],
                              Value = (string)col["value"]


                          })
                          .ToDictionary(c => c.Data);

        }
    }
    public class DataTableSearchs
    {
        public Dictionary<int, DataTableColumnSearch> Columns;


        private DataTableSearchs()
        {
        }

        /// <summary>
        /// Retrieve DataTable parameters from WebMethod parameter, sanitized against parameter spoofing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DataTableSearchs Get(object input)
        {
            return Get(JObject.FromObject(input));
        }

        /// <summary>
        /// Retrieve DataTable parameters from JSON, sanitized against parameter spoofing
        /// </summary>
        /// <param name="input">JToken object</param>
        /// <returns>parameters</returns>
        public static DataTableSearchs Get(JToken input)
        {
            return new DataTableSearchs
            {
                Columns = DataTableColumnSearch.Get(input),
            };
        }

        //public static DataTableSearchs Get(JToken input)
        //{
        //    return new DataTableParameters
        //    {
        //        Columns = DataTableColumn.Get(input_p),
        //        Order = DataTableOrder.Get(input_p),
        //        Draw = (int)input_p["draw"],
        //        Start = (int)input_p["start"],
        //        Length = (int)input_p["length"],
        //        SearchValue =
        //            new string(
        //                ((string)input_s["search"]["value"]).Where(
        //                    c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
        //        SearchRegex = (bool)input["search"]["regex"]
        //    };
        //}
    }
    public class DataTableParameters
    {
        public Dictionary<int, DataTableColumn> Columns;
        public int Draw;
        public int Length;
        public Dictionary<int, DataTableOrder> Order;
        public bool SearchRegex;
        public string SearchValue;
        public int Start;

        private DataTableParameters()
        {
        }

        /// <summary>
        /// Retrieve DataTable parameters from WebMethod parameter, sanitized against parameter spoofing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DataTableParameters Get(object input)
        {
            return Get(JObject.FromObject(input));
        }

        /// <summary>
        /// Retrieve DataTable parameters from JSON, sanitized against parameter spoofing
        /// </summary>
        /// <param name="input">JToken object</param>
        /// <returns>parameters</returns>
        public static DataTableParameters Get(JToken input)
        {
            return new DataTableParameters
            {
                Columns = DataTableColumn.Get(input),
                Order = DataTableOrder.Get(input),
                Draw = (int)input["draw"],
                Start = (int)input["start"],
                Length = (int)input["length"],
                SearchValue =
                    new string(
                        ((string)input["search"]["value"]).Where(
                            c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
                SearchRegex = (bool)input["search"]["regex"]
            };
        }

        //public static DataTableSearchs Get(JToken input)
        //{
        //    return new DataTableParameters
        //    {
        //        Columns = DataTableColumn.Get(input_p),
        //        Order = DataTableOrder.Get(input_p),
        //        Draw = (int)input_p["draw"],
        //        Start = (int)input_p["start"],
        //        Length = (int)input_p["length"],
        //        SearchValue =
        //            new string(
        //                ((string)input_s["search"]["value"]).Where(
        //                    c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
        //        SearchRegex = (bool)input["search"]["regex"]
        //    };
        //}
    }

    public class DataTableColumn
    {
        public int Data;
        public string Name;
        public bool Orderable;
        public bool Searchable;
        public bool SearchRegex;
        public string SearchValue;

        private DataTableColumn()
        {
        }

        /// <summary>
        /// Retrieve the DataTables Columns dictionary from a JSON parameter list
        /// </summary>
        /// <param name="input">JToken object</param>
        /// <returns>Dictionary of Column elements</returns>
        public static Dictionary<int, DataTableColumn> Get(JToken input)
        {
            var dc =

              ((JArray)input["columns"])
              .Select(col => new DataTableColumn
              {
                  Data = ((JArray)input["columns"]).IndexOf(col),
                  Name = (string)col["data"]
                  //Name =new string(((string)col["data"]).Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
                  //(int)col["data"],

              })
              .ToDictionary(c => c.Data);




            return (
                (JArray)input["columns"])
                .Select(col => new DataTableColumn
                {
                    Data = ((JArray)input["columns"]).IndexOf(col),
                    Name = (string)col["data"],
                    // (int)col["data"],
                    //Name =
                    //    new string(
                    //        ((string)col["data"]).Where(
                    //            c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
                    Searchable = (bool)col["searchable"],
                    Orderable = (bool)col["orderable"],
                    SearchValue =
                        new string(
                            ((string)col["search"]["value"]).Where(
                                c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray()),
                    SearchRegex = (bool)col["search"]["regex"]
                })
                .ToDictionary(c => c.Data);
        }
    }

    public class DataTableOrder
    {
        public int Column;
        public string Direction;

        private DataTableOrder()
        {
        }

        /// <summary>
        /// Retrieve the DataTables order dictionary from a JSON parameter list
        /// </summary>
        /// <param name="input">JToken object</param>
        /// <returns>Dictionary of Order elements</returns>
        public static Dictionary<int, DataTableOrder> Get(JToken input)
        {
            return (
                (JArray)input["order"])
                .Select(col => new DataTableOrder
                {
                    Column = (int)col["column"],
                    Direction =
                        ((string)col["dir"]).StartsWith("desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC"
                })
                .ToDictionary(c => c.Column);
        }
    }
}