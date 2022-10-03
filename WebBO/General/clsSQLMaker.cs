using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Text;

public class TSqlMaker
{
    // 欄位與欄位值在 List 內是位置互相對應的
    private List<string> Field = new List<string>(),  // Insert, Update 時的欄位
                         Value = new List<string>(),  // Insert, Update 時的欄位的值
                         WField = new List<string>(), // 有 Where 條件時的欄位
                         WValue = new List<string>(), // 有 Where 條件時的欄位的值
                         LField = new List<string>(), // 有 Like 條件時的欄位
                         LValue = new List<string>(); // 有 Like 條件時的欄位的值

    private string _TableName = ""; // 要操作的 Table

    public TSqlMaker()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }

    /// <summary>
    /// 要操作的 Table
    /// </summary>
    public string TableName
    {
        set
        {
            _TableName = value;
            Clear();
        }
    }

    /// <summary>
    /// 增加欄位和欄位的值
    /// </summary>
    /// <param name="aField">欄位</param>
    /// <param name="aValue">欄位的值</param>
    /// <param name="aValueSymbol">欄位的值用什麼符號括起來，一般為單引號</param>
    public void AddFieldValue(string aField, string aValue, string aValueSymbol)

    {
        string setN = "";
        if (aValueSymbol.Equals("'"))
            setN = "N";
        if (aField.Length <= 0) return;

        Field.Add(aField);
        Value.Add(setN + aValueSymbol + aValue + aValueSymbol);
    }

    public void AddFieldValue(string aField, string aValue)
    {
        this.AddFieldValue(aField, aValue, "");
    }

    /// <summary>
    /// 增加 Select 或 Update Where 條件欄位和欄位的值
    /// </summary>
    /// <param name="aField">欄位</param>
    /// <param name="aValue">欄位的值</param>
    /// <param name="aValueSymbol">欄位的值用什麼符號括起來，一般為單引號</param>
    public void AddWhereFieldValue(string aWField, string aWValue, string aWValueSymbol)
    {
        if (aWField.Length <= 0) return;

        WField.Add(aWField);
        WValue.Add(aWValueSymbol + aWValue + aWValueSymbol);
    }

    public void AddWhereFieldValue(string aWField, string aWValue)
    {
        AddWhereFieldValue(aWField, aWValue, "");
    }

    /// <summary>
    /// 增加 Select 或 Update Where-Like 條件欄位和欄位的值
    /// </summary>
    /// <param name="aField">欄位</param>
    /// <param name="aValue">欄位的值</param>
    /// <param name="aValueSymbol">欄位的值用什麼符號括起來，一般為單引號</param>
    public void AddLikeFieldValue(string aLField, string aLValue, string aLValueSymbol)
    {
        if (aLField.Length <= 0) return;

        WField.Add(aLField);
        WValue.Add(string.Format("{0}{1}{0}", aLValueSymbol, aLValue));
    }

    public void AddLikeFieldValue(string aLField, string aLValue)
    {
        AddLikeFieldValue(aLField, aLValue, "");
    }

    /// <summary>
    /// 清除所有欄位和欄位的值
    /// </summary>
    public void Clear()
    {
        Field.Clear();
        Value.Clear();
        WField.Clear();
        WValue.Clear();
        LField.Clear();
        LValue.Clear();
    }

    /// <summary>
    /// 產生 Update 的語法
    /// </summary>
    /// <returns>產生的語法</returns>
    public string GenUpdate()
    {
        if (_TableName.Length <= 0) return "";

        StringBuilder sbF1 = new StringBuilder(),
                      sbF2 = new StringBuilder();

        for (int i = 0; i < Field.Count; i++)
            sbF1.AppendFormat("[{0}]={1},", Field[i], Value[i]);

        for (int i = 0; i < WField.Count; i++)
            sbF2.AppendFormat("[{0}]={1} And", WField[i], WValue[i]);

        sbF1.Remove(sbF1.Length - 1, 1);
        sbF2.Remove(sbF2.Length - 4, 4);

        return string.Format("Update {0} Set {1} Where {2}", _TableName, sbF1.ToString(), sbF2.ToString());
    }

    ///// <summary>
    ///// 產生 Update 的語法
    ///// </summary>
    ///// <returns>產生的語法</returns>
    //public bool GenUpdate(SqlCommand aSqlCmd)
    //{
    //    if (_TableName.Length <= 0) return false;

    //    StringBuilder sbF1 = new StringBuilder(),
    //                  sbF2 = new StringBuilder();

    //    for (int i = 0; i < Field.Count; i++)
    //        sbF1.AppendFormat("[{0}]=@{0},", Field[i]);

    //    for (int i = 0; i < WField.Count; i++)
    //        sbF2.AppendFormat("[{0}]=@Where{0} And", WField[i]);

    //    sbF1.Remove(sbF1.Length - 1, 1);
    //    sbF2.Remove(sbF2.Length - 4, 4);

    //    string sql = string.Format("Update {0} Set {1} Where {2}", _TableName, sbF1.ToString(), sbF2.ToString());
    //    aSqlCmd.CommandText = sql;
    //    // AddFieldValue 時，aValueSymbol 必須給空字串
    //    for (int i = 0; i < Field.Count; i++)
    //        aSqlCmd.Parameters.Add(new SqlParameter("@" + Field[i], Value[i]));

    //    // AddWhereFieldValue 時，aValueSymbol 必須給空字串
    //    for (int i = 0; i < WField.Count; i++)
    //        aSqlCmd.Parameters.Add(new SqlParameter("@Where" + WField[i], WValue[i]));

    //    sbF1 = null;
    //    sbF2 = null;

    //    return true;
    //}

    /// <summary>
    /// 產生 Insert 的語法
    /// </summary>
    /// <returns>產生的語法</returns>
    public string GenInsert()
    {
        if (_TableName.Length <= 0) return "";

        StringBuilder sbF1 = new StringBuilder(),
                      sbF2 = new StringBuilder();

        for (int i = 0; i < Field.Count; i++)
        {
            sbF1.AppendFormat(string.Format("[{0}],", Field[i]));
            sbF2.AppendFormat(string.Format("{0},", Value[i]));
        }

        sbF1.Remove(sbF1.Length - 1, 1);
        sbF2.Remove(sbF2.Length - 1, 1);

        return string.Format("Insert Into {0}({1}) Values({2})", _TableName, sbF1.ToString(), sbF2.ToString());
    }

    ///// <summary>
    ///// 產生 Insert 的語法
    ///// </summary>
    ///// <returns>產生的語法</returns>
    //public bool GenInsert(SqlCommand aSqlCmd)
    //{
    //    if (_TableName.Length <= 0) return false;

    //    StringBuilder sbF1 = new StringBuilder(),
    //                  sbF2 = new StringBuilder();

    //    for (int i = 0; i < Field.Count; i++)
    //    {
    //        sbF1.AppendFormat(string.Format("[{0}],", Field[i]));
    //        sbF2.AppendFormat(string.Format("@{0},", Field[i]));
    //    }

    //    sbF1.Remove(sbF1.Length - 1, 1);
    //    sbF2.Remove(sbF2.Length - 1, 1);

    //    string sql = string.Format("Insert Into {0}({1}) Values({2})", _TableName, sbF1.ToString(), sbF2.ToString());

    //    aSqlCmd.CommandText = sql;
    //    // AddFieldValue 時，aValueSymbol 必須給空字串
    //    for (int i = 0; i < Field.Count; i++)
    //        aSqlCmd.Parameters.Add(new SqlParameter("@" + Field[i], Value[i]));

    //    sbF1 = null;
    //    sbF2 = null;

    //    return true;
    //}

    /// <summary>
    /// 產生 Count 的語法
    /// </summary>
    /// <returns>產生的語法</returns>
    public string GenCount()
    {
        if (_TableName.Length <= 0) return "";

        string f1 = "";
        if (Field.Count <= 0)
            f1 = "TCount";
        else
            f1 = Field[0];

        StringBuilder sbF2 = new StringBuilder();

        for (int i = 0; i < WField.Count; i++)
            sbF2.AppendFormat("[{0}]={1} And", WField[i], WValue[i]);

        sbF2.Remove(sbF2.Length - 4, 4);

        return string.Format("Select Count(*) As {0} From {1} Where {2}", f1, _TableName, sbF2.ToString()); ;
    }

    /// <summary>
    /// 產生 Delete 的語法
    /// </summary>
    /// <returns>產生的語法</returns>
    public string GenDelete()
    {
        if (_TableName.Length <= 0) return "";

        StringBuilder sbF2 = new StringBuilder();

        for (int i = 0; i < WField.Count; i++)
            sbF2.AppendFormat("[{0}]={1} And", WField[i], WValue[i]);

        sbF2.Remove(sbF2.Length - 4, 4);

        return string.Format("Delete From {0} Where {1}", _TableName, sbF2.ToString());
    }

    /// <summary>
    /// 產生 Select 的語法
    /// </summary>
    /// <returns>產生的語法</returns>
    public string GenSelect()
    {
        if (_TableName.Length <= 0) return "";

        StringBuilder sbF1 = new StringBuilder(),
                      sbF2 = new StringBuilder();

        for (int i = 0; i < Field.Count; i++)
            sbF1.AppendFormat("[{0}],", Field[i]);

        for (int i = 0; i < WField.Count; i++)
            sbF2.AppendFormat("[{0}]={1} And", WField[i], WValue[i]);

        sbF1.Remove(sbF1.Length - 1, 1);
        sbF2.Remove(sbF2.Length - 4, 4);

        return string.Format("Select {0} From {1} Where {2}", sbF1.ToString(), _TableName, sbF2.ToString());
    }

    ///// <summary>
    ///// 產生 Select 的語法
    ///// </summary>
    ///// <returns>產生的語法</returns>
    //public bool GenSelect(SqlCommand aSqlCmd)
    //{
    //    if (_TableName.Length <= 0) return false;

    //    StringBuilder sbF1 = new StringBuilder(),
    //                  sbF2 = new StringBuilder();

    //    for (int i = 0; i < Field.Count; i++)
    //        sbF1.AppendFormat("[{0}],", Field[i]);

    //    for (int i = 0; i < WField.Count; i++)
    //        sbF2.AppendFormat("[{0}]=@Where{0} And", WField[i]);

    //    for (int i = 0; i < LField.Count; i++)
    //        sbF2.AppendFormat("[{0}] Like @Like{0} And", LField[i]);

    //    sbF1.Remove(sbF1.Length - 1, 1);
    //    sbF2.Remove(sbF2.Length - 4, 4);

    //    string sql = string.Format("Select {0} From {1} Where {2}", sbF1.ToString(), _TableName, sbF2.ToString());
    //    aSqlCmd.CommandText = sql;
    //    // AddWhereFieldValue 時，aValueSymbol 必須給空字串
    //    for (int i = 0; i < WField.Count; i++)
    //        aSqlCmd.Parameters.Add(new SqlParameter("@Where" + WField[i], WValue[i]));

    //    for (int i = 0; i < LField.Count; i++)
    //        aSqlCmd.Parameters.Add(new SqlParameter("@Like" + WField[i], "%" + WValue[i] + "%"));

    //    sbF1 = null;
    //    sbF2 = null;

    //    return true;
    //}
}

