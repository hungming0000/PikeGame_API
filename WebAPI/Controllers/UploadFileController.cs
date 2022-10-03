using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http.Headers;
using System.Text;
using WebBO.General;
using WebBO.Models;
using WebBO.Services;
using isRock.Framework.WebAPI;

namespace WebAPI.Controllers
{

    public class UploadFileController : ApiController
    {
        private readonly string fileSavedPath = WebConfigurationManager.AppSettings["UploadPath"];
        private int gThumbPicHeight = 250;
        private int gThumbPicWidth = 250;

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            var message = "成功。";
            bool isSuccess = true;
            object data = null;

            HttpResponseMessage response = new HttpResponseMessage();

            var _uploadFileService = new UploadFileService();

            //try
            //{
            //    HttpFileCollection filestest = HttpContext.Current.Request.Files;
            //    //HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];


            //    var upFileID = string.Empty;


            //    // 處理圖片/檔案
            //    for (int i = 0; i < filestest.Count; i++)
            //    {
            //        HttpPostedFile postedFile = HttpContext.Current.Request.Files[i];

            //        // 檔案MIME內容型別
            //        var type = postedFile.ContentType.ToLower();

            //        // 圖片儲存路徑
            //        var folder = HttpContext.Current.Request.Form["UpVTFolder"];

            //        // 圖片.檔案名稱
            //        var actualFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            //        var fileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
            //        var fileExtName = Path.GetExtension(postedFile.FileName).ToLower();

            //        // 路徑
            //        var fullFilePath = $@"{fileSavedPath}\{folder}\{actualFileName}{fileExtName}";

            //        // 儲存檔案
            //        postedFile.SaveAs(fullFilePath);

            //        var thumbnailName = "";

            //        //if (fileExtName == ".jpg" || fileExtName == ".jpeg" || fileExtName == ".png" || fileExtName == ".gif")
            //        //{
            //        //    var fullThumbFilePath = $@"{fileSavedPath}\{folder}\{actualFileName}_tumb{fileExtName}";

            //        //    thumbnailName = actualFileName + "_tumb";

            //        //    // 儲存縮圖
            //        //    ImageTool.SaveThumbPicHeight(fullFilePath, gThumbPicWidth, fullThumbFilePath);
            //        //}

            //        // 取圖片資料
            //        var model = new EFAttachModel()
            //        {
            //            AttachId = HttpContext.Current.Request.Form["AttachId"],
            //            TableName = HttpContext.Current.Request.Form["TableName"],
            //            AttachRelatedId = HttpContext.Current.Request.Form["AttachRelatedId"],
            //            AttachLevel = HttpContext.Current.Request.Form["AttachLevel"],
            //            AttachName = fileName,
            //            AttachFileExt = fileExtName,
            //            UpVTFolder = folder,
            //            UpActualFileName = actualFileName,
            //            ModifyUser = HttpContext.Current.Request.Form["ModifyUser"],
            //            SysOrganizationID = HttpContext.Current.Request.Form["SysOrganizationID"],
            //        };

            //        // 上傳資料儲存到DB => tbUploadFile
            //        upFileID = _uploadFileService.UploadFile(model);
            //    }

            //    var _jsonString = new
            //    {
            //        isSuccess = isSuccess,
            //        Message = message,
            //        Data = data,
            //        Count = 1
            //    };

            //    var content = new StringContent(JsonConvert.SerializeObject(_jsonString), Encoding.UTF8, "application/json");

            //    response = new HttpResponseMessage
            //    {
            //        StatusCode = HttpStatusCode.OK,
            //        Content = content,
            //        ReasonPhrase = "Success"
            //    };

            //}
            //catch (Exception e)
            //{
            //    var _jsonString = new
            //    {
            //        isSuccess = false,
            //        Message = e.Message,
            //        Data = data,
            //        Count = 0
            //    };

            //    var content = new StringContent(JsonConvert.SerializeObject(_jsonString), Encoding.UTF8, "application/json");

            //    response = new HttpResponseMessage
            //    {
            //        StatusCode = HttpStatusCode.OK,
            //        Content = content,
            //        ReasonPhrase = "Upload File Error!"
            //    };
            //}

            return response;
        }
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[Route("UploadFile/{MethodName}")]
        //[HttpPost]
        //public IHttpActionResult ExecuteMethod(string MethodName)
        //{
        //    try
        //    {
        //        //AssemblyLauncher
        //        AssemblyLauncher assemblyLauncher = new AssemblyLauncher();
        //        //執行指定的Method

        //        var ret = assemblyLauncher.ExecuteCommand(new WebBO.Areas.EFinger.Controllers.UploadFileController(),
        //            MethodName,
        //            Request.Content.ReadAsStringAsync().Result);
        //        //回傳OK
        //        return Ok(ret);
        //    }
        //    catch (Exception ex)
        //    {
        //        //其他處理
        //        throw ex;
        //    }
        //}
    }
}