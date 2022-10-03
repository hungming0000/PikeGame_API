using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.General
{
    public class AppContractType
    {
        public DataTable GetAppContractType(string appFormID, string contractID)
        {
            var SQL = $@"select AppContractType from tbAppContractType 
                        where AppFormID = '{appFormID}' and ContractID = '{contractID}'";

            DataTable dt = CommonLib.GetDataFromSQL(SQL);

            return dt;

        }
    }
}
