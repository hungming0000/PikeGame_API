using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NLog;
using System.Globalization;
using System.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace WebBO.General
{
    using U_LLD = List<DataTable>;
    public class CommonLib
    {
        public static Logger WLogger = LogManager.GetLogger("WebAPI");
        public static List<Dictionary<string, object>> DataTableToListObject(DataTable dt)
        {
            #region  轉 DataTable 至 List<Dictionary<string,object>>
            List<Dictionary<string, object>> ListData = new List<Dictionary<string, object>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    item.Add(dt.Columns[j].ColumnName, dt.Rows[i][j]);
                }
                ListData.Add(item);
                item = null;
            }
            return ListData;
            #endregion
        }
        public static DBLayer GetDataLayer()
        {
            DBLayer dl = new DBLayer();
            dl.SetConnectName("WebDB");
            return dl;
        }
        //private DataTable _records;
        //public DataTable Records
        //{
        //    get { return _records; }
        //    set
        //    {
        //        if (_records == value) return;

        //        _records = value;
        //    }
        //}



        public CommonLib()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        /// <summary>
        /// 客戶端使用
        /// </summary>
        /// <param name="aDataList"></param>
        /// <param name="aDateTimeOnlyDate"></param>
        /// <returns></returns>
        //public static List<TBabuClass> MakeBabuList(ObservableCollection<Dictionary<string, string>> aDataList, bool aDateTimeOnlyDate = false)
        //{
        public static ObservableCollection<TBabuClass> MakeBabuList(System.Collections.Generic.List<Dictionary<string, string>> aDataList, bool aDateTimeOnlyDate = false)
        {
            TBabuClass bc = new TBabuClass();
            ObservableCollection<TBabuClass> LstData = new ObservableCollection<TBabuClass>();

            if (aDataList != null)
            {
                if (aDateTimeOnlyDate)
                {
                    OnlyDate(aDataList);
                }

                for (int i = 0; i < aDataList.Count; i++)
                {
                    LstData.Add(bc.CreateInstance(aDataList[i]));
                }
            }

            bc = null;

            return LstData;
        }

        /// <summary>
        /// 客戶端使用
        /// </summary>
        /// <param name="aDataList"></param>
        public static void OnlyDate(System.Collections.Generic.List<Dictionary<string, string>> aDataList)
        {
            if (aDataList != null)
            {
                List<string> keys = new List<string>();

                string timePattern1 = " 上午 12:00:00";
                string timePattern2 = " 下午 12:00:00";
                for (int i = 0; i < aDataList.Count; i++)
                {
                    for (int j = 0; j < aDataList[i].Count; j++)
                    {
                        keys.Clear();
                        foreach (string key in aDataList[i].Keys)
                        {
                            if (aDataList[i][key].IndexOf(timePattern1) != -1 ||
                                                                aDataList[i][key].IndexOf(timePattern2) != -1)
                            {
                                keys.Add(key);
                            }
                        }

                        foreach (string key in keys)
                        {
                            aDataList[i][key] = aDataList[i][key].Replace(timePattern1, "");
                            aDataList[i][key] = aDataList[i][key].Replace(timePattern2, "");
                        }
                    }
                }

                keys = null;
            }
        }


        /// <summary>
        /// 將 DataTable 轉為一個  List<Dictionary<string,string>> 的物件
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> DataTableToList(DataTable dt)
        {
            #region  轉 DataTable 至 List<Dictionary<string,string>>
            List<Dictionary<string, string>> ListData = new List<Dictionary<string, string>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    item.Add(dt.Columns[j].ColumnName, dt.Rows[i][j].ToString());
                }
                ListData.Add(item);
                item = null;
            }
            return ListData;
            #endregion
        }
        public static DataTable GetDataFromSQL(string SQLStr)
        {
            #region 依 SQL 指令，取回資料
            DataTable dt = new DataTable();
            lock (dt)
            {
                try
                {

                    DBLayer dl = CommonLib.GetDataLayer();
                    dt = dl.GetDataTable(SQLStr);
                    dt.TableName = string.Format("dt{0}", 0);
                    dl.Dispose();

                }
                catch (Exception ex)
                {
                    //CommonLib.WLogger.Error(ex.Message);
                    //var json = JsonConvert.SerializeObject(SQLStr);

                    //ProjectLogLib.CreateErrorLog("", ex, json);

                    throw;
                }



                #endregion
                return dt;
            }
        }

        public static DataStore GetDataListFromSQL(string[] SQLStr)
        {
            #region 依 SQL 指令，取回資料
            DataStore ds = new DataStore();
            lock (ds)
            {
                try
                {
                    ds.DataList = new U_LLD();
                    for (int i = 0; i < SQLStr.Length; i++)
                    {
                        // 沒有 SQL 語法的位置還是給空的資料集
                        if (SQLStr[i].Length <= 0)
                        {
                            DataTable dtzero = new DataTable();
                            dtzero.TableName = string.Format("dt{0}", i);
                            ds.DataList.Add(dtzero);
                            continue;
                        }
                        DBLayer dl = CommonLib.GetDataLayer();
                        DataTable dt = dl.GetDataTable(SQLStr[i]);
                        dt.TableName = string.Format("dt{0}", i);
                        ds.DataList.Add(dt);
                        dl.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    CommonLib.WLogger.Error(ex.Message);

                    //var json = JsonConvert.SerializeObject(SQLStr);

                    //LogRepository.CreateErrorLog(ex);

                    throw;
                }
            }
            return ds;
            #endregion
        }
        public static DataStore GetDataListFromSQL(System.Collections.ObjectModel.ObservableCollection<string> SQLStr)
        {
            #region 依 SQL 指令，取回資料
            DataStore ds = new DataStore();
            lock (ds)
            {
                try
                {
                    ds.DataList = new U_LLD();
                    for (int i = 0; i < SQLStr.Count; i++)
                    {
                        // 沒有 SQL 語法的位置還是給空的資料集
                        if (SQLStr[i].Length <= 0)
                        {
                            DataTable dtzero = new DataTable();
                            dtzero.TableName = string.Format("dt{0}", i);
                            ds.DataList.Add(dtzero);
                            continue;
                        }
                        DBLayer dl = CommonLib.GetDataLayer();
                        DataTable dt = dl.GetDataTable(SQLStr[i]);
                        dt.TableName = string.Format("dt{0}", i);
                        ds.DataList.Add(dt);
                        dl.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    CommonLib.WLogger.Error(ex.Message);
                    throw ex;
                }
            }
            return ds;
            #endregion
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 資料包裝
        /// </summary>
        public class DataStore
        {
            #region 資料包裝
            /// <summary>
            /// 資料結果實體
            /// </summary>
            public DataTable Data;
            /// <summary>
            /// 資料結果實體
            /// </summary>
            public U_LLD DataList;
            /// <summary>
            /// 欄位資訊
            /// </summary>
            public List<string> FieldList;
            #endregion
        }


        //--------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 日期公用函數
        /// </summary>
        //日期的幾種格式
        static string[] DateTimeList = {
                            "yyyy/M/d tt hh:mm:ss",
                            "yyyy/MM/dd tt hh:mm:ss",
                            "yyyy/MM/dd HH:mm:ss",
                            "yyyy/M/d HH:mm:ss",
                            "yyyy/M/d",
                            "yyyy/MM/dd" ,
                            "yyyy-MM-dd" ,
                            "yyyy-M-d"
                        };

        //轉換西元日期到民國年(yyy/mm/dd)
        public static string ConEngDateToChtDate(string EngDateStr, string JoinChar = "/", string Year23 = "3")
        {
            string scv = "";
            DateTime d;
            IFormatProvider ifp = new CultureInfo("zh-tw");
            if (DateTime.TryParseExact(EngDateStr, DateTimeList, ifp, DateTimeStyles.AllowWhiteSpaces, out d))
            {
                if (Year23 == "2")
                {
                    if ((d.Year - 1911) > 100)
                    {
                        scv = (d.Year - 1911 - 100).ToString("00") + JoinChar + d.Month.ToString("00") + JoinChar + d.Day.ToString("00");
                    }
                    else
                    {
                        scv = (d.Year - 1911).ToString("00") + JoinChar + d.Month.ToString("00") + JoinChar + d.Day.ToString("00");
                    }
                }
                else
                {
                    scv = (d.Year - 1911).ToString("000") + JoinChar + d.Month.ToString("00") + JoinChar + d.Day.ToString("00");
                }
            }
            else
            {
                scv = "";
            }
            return scv;
        }

        //轉換西元日期到民國年(yyy/mm/dd + 時間)
        public static string ConEngDateToChtDate(string EngDateStr, bool showTime)
        {
            string scv = "";
            if (showTime)
            {
                DateTime d;
                IFormatProvider ifp = new CultureInfo("zh-tw");
                if (DateTime.TryParseExact(EngDateStr, DateTimeList, ifp, DateTimeStyles.AllowWhiteSpaces, out d))
                {
                    scv = (d.Year - 1911).ToString("000") + "/" + d.Month.ToString("00") + "/" + d.Day.ToString("00") + d.ToShortTimeString();
                }
                else
                {
                    scv = "";
                }
            }
            return scv;
        }

        //轉換西元時間到民國時間(yyy/mm/dd)
        public static string ConEngTimeToChtTime(string EngTimeStr)
        {
            string scv = "";
            DateTime d;
            IFormatProvider ifp = new CultureInfo("zh-tw");
            if (DateTime.TryParseExact(EngTimeStr, DateTimeList, ifp, DateTimeStyles.AllowWhiteSpaces, out d))
            {
                scv = (d.Year - 1911).ToString("000") + "/" + d.Month.ToString("00") + "/" + d.Day.ToString("00") + " " + d.ToString("HH:mm:ss");
            }
            else
            {
                scv = "";
            }
            return scv;
        }


        //轉換西元月份到民國月(yyy/mm/dd)
        public static string ConEngMonthToChtMonth(string EngDateStr)
        {
            string scv = "";
            DateTime d;
            IFormatProvider ifp = new CultureInfo("zh-tw");
            if (DateTime.TryParseExact(EngDateStr, DateTimeList, ifp, DateTimeStyles.AllowWhiteSpaces, out d))
            {
                scv = (d.Year - 1911).ToString("000") + "/" + d.Month.ToString("00");
            }
            else
            {
                scv = "";
            }
            return scv;
        }

        //轉換民國日期到西元年(098/12/31==2009/12/31)
        public static string ConChtDateToEngDate(string ChtDate)
        {
            string scv = "";
            string[] aTempDate = ChtDate.Split('/');
            try
            {
                scv = (1911 + Convert.ToInt16(aTempDate[0])).ToString() + "/" + aTempDate[1] + "/" + aTempDate[2];
            }
            catch
            {
                scv = "";
            }
            return scv;
        }

        //轉換西元日期到民國格式,並且可取得年,月,日
        public static string ConEngDateToChtYMD(string EngDateStr, string ReturnYMD)
        {
            string scv = "";
            DateTime d;
            IFormatProvider ifp = new CultureInfo("zh-tw");
            if (DateTime.TryParseExact(EngDateStr, DateTimeList, ifp, DateTimeStyles.AllowWhiteSpaces, out d))
            {
                switch (ReturnYMD)
                {
                    case "Y":
                        scv = (d.Year - 1911).ToString("000");
                        break;
                    case "M":
                        scv = d.Month.ToString("00");
                        break;
                    case "D":
                        scv = d.Day.ToString("00");
                        break;
                    default:
                        scv = (d.Year - 1911).ToString("000") + "/" + d.Month.ToString("00") + "/" + d.Day.ToString("00");
                        break;
                }
            }
            else
            {
                scv = "";
            }
            return scv;
        }

        /// <summary>
        /// 檢查身分證的合法性
        /// </summary>
        /// <param name="vid"></param>
        /// <returns>1：字數不到10，2：第二碼非1,2，3：首碼有誤，4：查碼不對，０：正確</returns>
        public static string ChackIdentityCard(string vid)
        {
            List<string> FirstEng =
              new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J",
                           "K", "L", "M", "N", "P", "Q", "R", "S", "T",
                           "U", "V", "X", "Y", "W", "Z", "I", "O" };

            string aa = vid.ToUpper();
            bool chackFirstEnd = false;
            if (aa.Trim().Length == 10)
            {
                byte firstNo = Convert.ToByte(aa.Trim().Substring(1, 1));
                if (firstNo > 2 || firstNo < 1)
                {
                    return "2";
                }
                else
                {
                    int x;
                    for (x = 0; x < FirstEng.Count; x++)
                    {
                        if (aa.Substring(0, 1) == FirstEng[x])
                        {
                            aa = string.Format("{0}{1}", x + 10, aa.Substring(1, 9));
                            chackFirstEnd = true;
                            break;
                        }
                    }

                    if (!chackFirstEnd)
                        return "3";

                    int i = 1;
                    int ss = int.Parse(aa.Substring(0, 1));

                    while (aa.Length > i)
                    {
                        ss = ss + (int.Parse(aa.Substring(i, 1)) * (10 - i));
                        i++;
                    }

                    aa = ss.ToString();

                    if (vid.Substring(9, 1) == "0")
                    {
                        if (aa.Substring(aa.Length - 1, 1) == "0")
                        {
                            return "0";
                        }
                        else
                        {
                            return "4";
                        }
                    }
                    else
                    {
                        if (vid.Substring(9, 1) ==
                          (10 - int.Parse(aa.Substring(aa.Length - 1, 1))).ToString())
                        {
                            return "0";
                        }
                        else
                        {
                            return "4";
                        }
                    }
                }
            }
            else
            {
                return "1";
            }
        }


        /// <summary>
        /// 檢查統一編號合法性
        /// </summary>
        /// <param name="sCompanyID"></param>
        /// <returns></returns>
        public static bool CheckCompanyID(string sCompanyID)
        {
            try
            {
                //傳入公司統編長度不等於8就return
                if (sCompanyID.Length != 8)
                    return false;

                int aSum = 0;

                //公司統編邏輯乘數( 1, 2, 1, 2, 1, 2, 4, 1 )
                int[] LogicCompanyID = { 1, 2, 1, 2, 1, 2, 4, 1 };

                for (int i = 0; i < LogicCompanyID.Length; i++)
                {
                    //公司統編與邏輯乘數相乘
                    int aMultiply = Convert.ToInt32(sCompanyID.Substring(i, 1)) * LogicCompanyID[i];

                    //將相乘的結果, 取十位數及個位數相加
                    int aAddition = ((aMultiply / 10) + (aMultiply % 10));

                    //如果公司統編的第 7 位是 7 時, 會造成相加結果為 10 的特殊情況, 所以直接以 0 代替進行加總
                    aSum += (aAddition == 10) ? 0 : aAddition;

                }
                //判斷總和的餘數, 假使為 0 公司統編正確回傳 true, 其它值則反之.
                return (aSum % 10 == 0);
            }
            catch
            {
                return false;
            }
        }
    }
}