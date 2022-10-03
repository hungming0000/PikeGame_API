using System;
using System.Threading.Tasks;
using WebBO.Controllers;
using WebBO.General;

namespace WebAPI.Providers
{
    public class ADAuth : IDisposable
    {

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<UserInfo> FindUser(string Acc, string Pwd)
        {
            UserInfo Result = new UserInfo();
            try
            {
                Result = UserInfo.VerifyUserInfo(Acc, Pwd);
            }
            catch (Exception Ex)
            {
                CommonLib.WLogger.Error("Login Exception：" + Ex.Message);
                throw new Exception(Ex.Message);
            }

            return Result;
        }
    }
}