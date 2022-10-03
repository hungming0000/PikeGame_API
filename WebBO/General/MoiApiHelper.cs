using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebBO.General
{
    public class MoiApiHelper
    {
        //應用系統識別碼
        static string _applicationId = "9561845e-03cb-4f69-a819-6c774bc52967";

        //應用系統密碼
        static string _appPWD = "1qazxsw2";

        //proxy的位址
        static string _proxyUrl = "http://proxy3.forest:8080";


        //公司內部測試的介接帳號
        //static string _applicationId = "hialinya@gmail.com";
        //static string _appPWD = "1qaZxsw23edc";
        //static string _proxyUrl = "";

        //共享平台 API 網址 + 服務的命名空間
        //static string _url = "http://lisp.land.moi.gov.tw/cp/api.ashx/";

        //新的地政整合資訊共享平台服務介接網址 2019/02/06 by Alin 
        static string _url = "https://api.land.moi.gov.tw/cp/api/";

        //所要操作的API命名空間
        static string _defaultNS = "CadastralData";

        private static HttpWebRequest CreateRequest(string url)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            //忽略ssl憑證檢查
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            Uri _uri = new Uri(url);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_uri);

            //有proxy則必須要設定
            if (_proxyUrl != "")
                request.Proxy = new System.Net.WebProxy(_proxyUrl, true);

            request.KeepAlive = false;

            request.ProtocolVersion = HttpVersion.Version10;

            //將應用系統識別碼 及 密碼 用 「：」連接起來成為一個字串
            string _credString = string.Format("{0}:{1}", _applicationId, _appPWD);

            //將該字串轉為二進位資料
            Byte[] _credByte = Encoding.ASCII.GetBytes(_credString);

            //將前述的二進位資料轉換為 base64 字串格式
            string _credBase64 = Convert.ToBase64String(_credByte);

            //將前述的字串放進 http header
            request.Headers.Add("Authorization", "Basic " + _credBase64);

            //設定 contenttype 為 json
            request.ContentType = "application/json; charset=utf-8";

            //設定使用 post 方式發送要求
            request.Method = "POST";

            return request;
        }

        public static string Request(string NS, string method, string json)
        {
            string _result;

            //建立指定方法的 httprequest 物件
            HttpWebRequest request = CreateRequest(_url + "/" + NS + "/" + method);

            //將json資料寫進 httprequest的body中
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            //取得API執行的結果
            var httpResponse = (HttpWebResponse)request.GetResponse();

            //取出回傳的資料內容
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                _result = streamReader.ReadToEnd();
            }

            return _result;
        }

        public static string Request(string method, string json)
        {
            string _result;

            //建立指定方法的 httprequest 物件
            HttpWebRequest request = CreateRequest(_url + "/" + _defaultNS + "/" + method);

            //將json資料寫進 httprequest的body中
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            //取得API執行的結果
            var httpResponse = (HttpWebResponse)request.GetResponse();

            //取出回傳的資料內容
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                _result = streamReader.ReadToEnd();
            }

            return _result;
        }
    }
}
