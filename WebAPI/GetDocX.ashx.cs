using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using DocXParser;
namespace WebAPI
{
    /// <summary>
    ///GetDocX 的摘要描述
    /// </summary>
    public class GetDocX : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //讀取傳過來的參數
                string ContractID = HttpContext.Current.Request.QueryString["ContractID"];
                string DocName = HttpContext.Current.Request.QueryString["DocName"];
                string ParamterKeys = HttpContext.Current.Request.QueryString["ParamterKeys"];
                string ContractType = HttpContext.Current.Request.QueryString["ContractType"];
                string UserID = HttpContext.Current.Request.QueryString["UserID"];
                //if (context.Request.Browser.Browser.ToLower().Contains("firefox"))

                //解碼
                ContractID = HttpUtility.UrlDecode(ContractID);
                DocName = HttpUtility.UrlDecode(DocName);
                ParamterKeys = HttpUtility.UrlDecode(ParamterKeys);
                ContractType = HttpUtility.UrlDecode(ContractType);
                UserID = HttpUtility.UrlDecode(UserID);

                ProcessDocX.Connstring = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();
                ProcessDocX pd = new ProcessDocX();
                MemoryStream ms = pd.GetDocX(ContractID, DocName, ParamterKeys, ContractType, UserID);

                //
                if (ms.Length > 0)
                {
                    ms.Position = 0;
                    context.Response.ContentType = "application/docx";
                    DocName += ".docx";
                    if (context.Request.Browser.Browser == "IE" | context.Request.Browser.Browser.ToLower().Contains("internetexplorer"))
                    {
                        DocName = context.Server.UrlPathEncode(DocName);
                    }
                    string strContentDisposition = String.Format("{0}; filename=\"{1}\"", "attachment", DocName);
                    context.Response.AddHeader("Content-Disposition", strContentDisposition);
                    //context.Response.AppendHeader("content-disposition", "inline; filename=" + HttpUtility.UrlEncode(DocName, System.Text.Encoding.UTF8));
                    context.Response.Clear();
                    context.Response.BufferOutput = true;
                    context.Response.BinaryWrite(ms.ToArray());
                }
                else
                {                    
                    context.Response.ContentType = "text/html";                    
                    context.Response.Write("<h1>文件可能不存在</h1>");
                    context.Response.Write("ContractID:" + ContractID);
                    context.Response.Write("DocName:" + HttpUtility.UrlEncode(DocName,System.Text.Encoding.UTF8));
                    context.Response.Write("ParamterKeys:" + ParamterKeys);
                    context.Response.Write("ContractType:" + ContractType);
                }
            }
            catch (Exception Ex)
            {
                context.Response.ContentType = "text/html";
                context.Response.Write("<h1>文件資料轉換發生下列錯誤：</h1>");
                context.Response.Write(Ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }       
    }

}