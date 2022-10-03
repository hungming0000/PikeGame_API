using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Data;
using System.Dynamic;
using System.Diagnostics;
using System.Threading;
using System.Web.Configuration;

namespace WebBO.General
{
    public static class ImageTool
    {
        private static string fileSavedPath = WebConfigurationManager.AppSettings["UploadPath"];

        /// <summary>
        /// 將db內的base64字串轉成指定的縮圖大小(等比例)
        /// </summary>
        /// <param name="base64String">資料庫內的base64字串</param>
        /// <param name="ContentType">檔案類型</param>
        /// <param name="width">縮圖寬度</param>
        /// <param name="height">縮圖高度</param>
        /// <returns></returns>
        public static byte[] Base64ToThumbImage(string base64String, string ContentType, int width, int height)
        {
            Image original = Base64ToImage(base64String);

            //等比例
            if (original.Width > original.Height)
            {
                height = original.Height * width / original.Width;
            }
            else
            {
                width = original.Width * height / original.Height;
            }

            Image img = original.GetThumbnailImage(width, height, null, System.IntPtr.Zero);
            MemoryStream mm = new MemoryStream();
            switch (ContentType.ToLower())
            {
                case ".bmp":
                    img.Save(mm, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case ".jpg":
                    img.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".png":
                    img.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case ".tif":
                    img.Save(mm, System.Drawing.Imaging.ImageFormat.Tiff);
                    break;
                case ".gif":
                    img.Save(mm, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                default:
                    img.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
            }

            byte[] bytesImage = mm.GetBuffer();

            img.Dispose();
            original.Dispose();
            mm.Dispose();

            return bytesImage;
        }

        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public static void Base64ToFile(string base64String, string fileName)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            FileStream myFile = File.Open(fileName, FileMode.OpenOrCreate);
            ms.WriteTo(myFile);

            myFile.Dispose();
            ms.Dispose();
        }

        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        #region 另存縮圖 by Alin 2020/02/04

        #region  取得圖片等比例縮圖後的寬和高像素
        /// <summary>
        ///  寬高誰較長就縮誰  - 計算方法
        /// </summary>
        /// <param name="bmp">System.Drawing.Image 的物件</param>
        /// <param name="maxPx">寬或高超過多少像素就要縮圖</param>
        /// <returns>回傳int陣列，索引0為縮圖後的寬度、索引1為縮圖後的高度</returns>
        public static int[] GetThumbPic_WidthAndHeight(Bitmap bmp, int maxPx)
        {
            int newWidth = 0;
            int newHeight = 0;

            if (bmp.Width > maxPx || bmp.Height > maxPx)
            //如果圖片的寬大於最大值或高大於最大值就往下執行  
            {
                if (bmp.Width >= bmp.Height)
                //圖片的寬大於圖片的高  
                {
                    newHeight = Convert.ToInt32((Convert.ToDouble(maxPx) / Convert.ToDouble(bmp.Width)) * Convert.ToDouble(bmp.Height));
                    //設定修改後的圖高  
                    newWidth = maxPx;
                }
                else
                {
                    newWidth = Convert.ToInt32((Convert.ToDouble(maxPx) / Convert.ToDouble(bmp.Height)) * Convert.ToDouble(bmp.Width));
                    //設定修改後的圖寬  
                    newHeight = maxPx;
                }
            }
            else
            {//圖片沒有超過設定值，不執行縮圖   
                newHeight = bmp.Height;
                newWidth = bmp.Width;
            }

            int[] newWidthAndfixHeight = { newWidth, newHeight };

            return newWidthAndfixHeight;
        }

        /// <summary>
        /// 寬度維持maxWidth，高度等比例縮放   - 計算方法
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static int[] GetThumbPic_Width(Bitmap bmp, int maxWidth)
        {
            //要回傳的結果
            int newWidth = 0;
            int newHeight = 0;

            if (bmp.Width > maxWidth)
            //如果圖片的寬大於最大值 
            {
                //等比例的圖高
                newHeight = Convert.ToInt32((Convert.ToDouble(maxWidth) / Convert.ToDouble(bmp.Width)) * Convert.ToDouble(bmp.Height));
                //設定修改後的圖寬  
                newWidth = maxWidth;
            }
            else
            {//圖片寬沒有超過設定值，不執行縮圖  
                newHeight = bmp.Height;
                newWidth = bmp.Width;
            }

            int[] newWidthAndfixHeight = { newWidth, newHeight };

            return newWidthAndfixHeight;
        }

        /// <summary>
        /// 高度維持maxHeight，寬度等比例縮放  - 計算方法
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static int[] GetThumbPic_Height(Bitmap bmp, int maxHeight)
        {
            //要回傳的值
            int newWidth = 0;
            int newHeight = 0;

            if (bmp.Height > maxHeight)
            //如果圖片的高大於最大值 
            {
                //等比例的寬
                newWidth = Convert.ToInt32((Convert.ToDouble(maxHeight) / Convert.ToDouble(bmp.Height)) * Convert.ToDouble(bmp.Width));
                //圖高固定  
                newHeight = maxHeight;
            }
            else
            {
                //圖片的高沒有超過設定值
                newHeight = bmp.Height;
                newWidth = bmp.Width;
            }

            int[] newWidthAndfixHeight = { newWidth, newHeight };

            return newWidthAndfixHeight;
        }
        #endregion

        #region 產生縮圖並儲存
        /// <summary>
        /// 產生縮圖並儲存 寬高誰較長就縮誰
        /// </summary>
        /// <param name="srcImagePath">來源圖片的路徑</param>
        /// <param name="maxPix">超過多少像素就要等比例縮圖</param>
        /// <param name="saveThumbFilePath">縮圖的儲存檔案路徑</param>
        public static void SaveThumbPic(string srcImagePath, int maxPix, string saveThumbFilePath)
        {
            //讀取原始圖片 
            using (FileStream fs = new FileStream(srcImagePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                //取得原始圖片 
                Bitmap bmpOld = new Bitmap(fs);

                // 計算維持比例的縮圖大小 
                int[] thumbnailScaleWidth = GetThumbPic_WidthAndHeight(bmpOld, maxPix);
                int newWidth = thumbnailScaleWidth[0];
                int newHeight = thumbnailScaleWidth[1];

                // 產生縮圖 
                Bitmap bmpThumb = new Bitmap(bmpOld, newWidth, newHeight);
                bmpThumb.Save(saveThumbFilePath);

            }//end using 
        }

        /// <summary>
        /// 產生縮圖並儲存 寬度維持maxpix，高度等比例
        /// </summary>
        /// <param name="srcImagePath">來源圖片的路徑</param>
        /// <param name="widthMaxPix">超過多少像素就要等比例縮圖</param>
        /// <param name="saveThumbFilePath">縮圖的儲存檔案路徑</param>
        public static void SaveThumbPicWidth(string srcImagePath, int widthMaxPix, string saveThumbFilePath)
        {
            //讀取原始圖片 
            using (FileStream fs = new FileStream(srcImagePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                //取得原始圖片 
                Bitmap bmpOld = new Bitmap(fs);

                //圖片寬高 
                // 計算維持比例的縮圖大小 
                int[] thumbnailScaleWidth = GetThumbPic_Width(bmpOld, widthMaxPix);
                int newWidth = thumbnailScaleWidth[0];
                int newHeight = thumbnailScaleWidth[1];

                // 產生縮圖 
                Bitmap bmpThumb = new Bitmap(bmpOld, newWidth, newHeight);
                bmpThumb.Save(saveThumbFilePath);

            }//end using
        }

        /// <summary>
        /// 產生縮圖並儲存 高度維持maxPix，寬度等比例
        /// </summary>
        /// <param name="srcImagePath">來源圖片的路徑</param>
        /// <param name="heightMaxPix">超過多少像素就要等比例縮圖</param>
        /// <param name="saveThumbFilePath">縮圖的儲存檔案路徑</param>
        public static void SaveThumbPicHeight(string srcImagePath, int heightMaxPix, string saveThumbFilePath)
        {
            //讀取原始圖片 
            using (FileStream fs = new FileStream(srcImagePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                //取得原始圖片 
                Bitmap bmpOld = new Bitmap(fs);

                // 計算維持比例的縮圖大小 
                int[] thumbnailScaleWidth = GetThumbPic_Height(bmpOld, heightMaxPix);
                int newWidth = thumbnailScaleWidth[0];
                int newHeight = thumbnailScaleWidth[1];

                // 產生縮圖 
                Bitmap bmpThumb = new Bitmap(bmpOld, newWidth, newHeight);
                bmpThumb.Save(saveThumbFilePath);

            }//end using 
        }

        #endregion

        #endregion

        #region 刪除檔案
        public static void DeleteFile(DataTable dt)
        {
            try
            {
                //var path = AppDomain.CurrentDomain.BaseDirectory;

                // 路徑
                var fileName = $"{dt.Rows[0]["UpActualFileName"]}{dt.Rows[0]["AttachFileExt"]}";

                //var thumbFileName = $"{dt.Rows[0]["UpThumbnailName"]}{dt.Rows[0]["AttachFileExt"]}";

                var fullFilePath = $"{fileSavedPath}{dt.Rows[0]["UpVTFolder"]}/{fileName}";

                //var thumbFullFilePath = $"{fileSavedPath}{dt.Rows[0]["UpVTFolder"]}/{thumbFileName}";

                if (File.Exists(fullFilePath))
                {
                    File.Delete(fullFilePath);
                    //File.Delete(thumbFullFilePath);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion
    }

}
