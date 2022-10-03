using DataTables;
using isRock.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBO.DataClass;
using WebBO.General;
using WebBO.Models;

namespace WebBO.Controllers
{
    public class UserInfoController : BusinessLogicBase
    {
        #region 個人資料 GetUserDataID
        public ExecuteCommandAPIResult GetUserDataID(string userID)
        {
            var message = "成功。";
            bool isSuccess = true;

            var SQL = $@"select *
                        from V_SysUserInfo
                        where UserID = '{userID}'";

            DataTable dt = CommonLib.GetDataFromSQL(SQL);

            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = dt,
                Count = dt.Rows.Count,
            };
        }
        #endregion

        #region 更新個人資料 SaveUserInfo
        public ExecuteCommandAPIResult SaveUserInfo(UserInfoModel request)
        {
            var message = "成功。";
            bool isSuccess = true;

            var SQL = $@"update tbSysUserData set 
                            IsDirector = '{request.IsDirector}',
                            Email = '{request.Email}',
                            Tel = '{request.Tel}',
                            CellPhone = '{request.CellPhone}',
                            Memo = '{request.Memo}',
                            ModifyUser = '{request.UserID}',
                            ModifyDate = getdate()
                            where UserID = '{request.UserID}'";

            DataTable dt = CommonLib.GetDataFromSQL(SQL);

            return new ExecuteCommandAPIResult()
            {
                isSuccess = isSuccess,
                Message = message,
                Data = dt,
                Count = dt.Rows.Count,
            };
        }
        #endregion


        public class ChangePassWord
        {
            public string Password { get; set; }
            public string UserID { get; set; }
        }
        #region 變更密碼 ChangePassword
        public ExecuteCommandAPIResult ChangePassword(ChangePassWord request)
        {
            var message = "成功。";
            bool isSuccess = true;

            var SQL = $@"update tbSysUserData set 
                            Password='{request.Password}',
                            LastResetPwTime = getdate()
                            where UserID = '{request.UserID}'";

            DataTable dt = CommonLib.GetDataFromSQL(SQL);

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
}
