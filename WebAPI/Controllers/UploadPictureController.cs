using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using WebBO.General;
using WebBO.Models;
using WebBO.Services;

namespace WebAPI.Controllers
{
    public class UploadPictureController : ApiController
    {
        private readonly string fileSavedPath = WebConfigurationManager.AppSettings["UploadPath"];
        private int gThumbPicHeight = 250;
        private int gThumbPicWidth = 250;

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            //var message = "成功。";
            //bool isSuccess = true;
            //object data = null;

            //HttpResponseMessage response = new HttpResponseMessage();

            //var _uploadFileService = new UploadFileService();

            //try
            //{
            //    HttpFileCollection filestest = HttpContext.Current.Request.Files;

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

            //        if (fileExtName == ".jpg" || fileExtName == ".jpeg" || fileExtName == ".png" || fileExtName == ".gif")
            //        {
            //            var fullThumbFilePath = $@"{fileSavedPath}\{folder}\{actualFileName}_tumb{fileExtName}";

            //            thumbnailName = actualFileName + "_tumb";

            //            // 儲存縮圖
            //            ImageTool.SaveThumbPicHeight(fullFilePath, gThumbPicWidth, fullThumbFilePath);
            //        }

            //        // 取圖片資料
            //        var model = new ProcessingStepModel()
            //        {
            //            ProcessingStepId  = HttpContext.Current.Request.Form["ProcessingStepId"],
            //            PictureName = fileName,
            //            FlowId   =HttpContext.Current.Request.Form["FlowId"],
            //            PictureFileExt = fileExtName,
            //            UpVTFolder = folder,
            //            UpActualFileName = actualFileName,
            //            UpThumbnailName = thumbnailName,
            //            ModifyUser = HttpContext.Current.Request.Form["ModifyUser"],

            //        };

            //        // 上傳資料儲存到DB => tbUploadFile
            //        upFileID = _uploadFileService.UploadPirture(model);
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

            //return response;
            return null;
        }
    }
}