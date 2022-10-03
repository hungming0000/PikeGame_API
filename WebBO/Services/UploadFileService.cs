using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBO.General;
using WebBO.Models;

namespace WebBO.Services
{
    public class UploadFileService
    {
        #region 上傳檔案
        //public string UploadFile(EFAttachModel file)
        //{
        //    var AttachId = Guid.NewGuid().ToString("N");

        //    var SQL = $@" INSERT INTO tbEFAttach (
	       //                 AttachId
	       //                 ,AttachName
        //                    ,TableName
	       //                 ,AttachRelatedId
	       //                 ,AttachLevel
	       //                 ,AttachData
        //                    ,AttachFileExt
        //                    ,AttachFileSize
        //                    ,UpActualFileName
        //                    ,UpVTFolder
	       //                 ,ModifyDate
	       //                 ,ModifyUser
        //                    ,SysOrganizationID
	       //                 )
        //                VALUES (
        //                    '{AttachId}',
        //                    '{file.AttachName}',
        //                    '{file.TableName}',
        //                    '{file.AttachRelatedId}',
        //                    '{file.AttachLevel}',
        //                    '{file.AttachData}',
        //                    '{file.AttachFileExt}',
        //                    '{file.AttachFileSize}',
        //                    '{file.UpActualFileName}',
        //                    '{file.UpVTFolder}',
        //                     getdate() ,
        //                    '{file.ModifyUser}',
        //                    '{file.SysOrganizationID}'
        //                )";
        //    CommonLib.GetDataFromSQL(SQL);

        //    if (file.AttachLevel == "L0")
        //    {
        //        var LazyAttachId = Guid.NewGuid().ToString("N");
        //        SQL = $@"
        //                INSERT INTO tbEFLazyFromAttach (
	       //                 LazyAttachId
	       //                 ,LazyId
	       //                 ,AttachId
	       //                 )
        //                VALUES (
        //                    '{LazyAttachId}',
        //                    '{file.AttachRelatedId}',
        //                    '{AttachId}'
        //                );
        //        ";
        //        CommonLib.GetDataFromSQL(SQL);

        //    }

        //    return AttachId;
        //}
        #endregion
        //#region 上傳流程圖
        //public string UploadPirture(ProcessingStepModel file)
        //{
        //    var ProcessingStepId = Guid.NewGuid().ToString("N");

        //    var SQL = $@"  INSERT INTO tbEFProcessingStep (
	       //             ProcessingStepId
	       //             ,FlowId
	       //             ,PictureName
	       //             ,PictureData
        //                ,PictureSize
        //                ,UpActualFileName
        //                ,PictureFileExt
        //                ,UpThumbnailName
        //                ,UpVTFolder
	       //             ,ModifyUser
	       //             ,ModifyDate
	       //             )
        //            VALUES (
        //                '{ProcessingStepId}',
        //                '{file.FlowId}',
        //                '{file.PictureName}',
        //                '{file.PictureData}',
        //                '{file.PictureSize}',
        //                '{file.UpActualFileName}',
        //                '{file.PictureFileExt}',
        //                '{file.UpThumbnailName}',
        //                '{file.UpVTFolder}',
        //                '{file.ModifyUser}',
        //                 getdate()                      

        //            );
 
        //                select * from tbEFProcessingStep
        //                where ProcessingStepId='{ProcessingStepId}';
        //                ";
        //    CommonLib.GetDataFromSQL(SQL);

        //    return ProcessingStepId;
        //}
        //#endregion

    }
}
