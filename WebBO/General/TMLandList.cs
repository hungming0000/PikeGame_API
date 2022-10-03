using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.General
{
    public class TMLandList
    {
        // <summary>
        /// 紀錄重測前地籍資料
        /// </summary>
        /// <param name="nsContractID"></param>
        /// <returns></returns>
        public bool CheckIsNSScheduleData_Plan(string nsContractID)
        {
            var SQL = $@"select cs.NSContractScheduleID, cs.NSScheduleDataPlanID, sd.NSScheduleDataID
                            from tbNSContractSchedule as cs
                            left join tbNSScheduleData as sd
                            on cs.NSContractScheduleID = sd.NSContractScheduleID
                            where cs.NSContractID = '{nsContractID}'";

            var dt = CommonLib.GetDataFromSQL(SQL);

            //return dt;

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["NSScheduleDataID"] == null || dt.Rows[0]["NSScheduleDataID"].ToString() == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }
    }
}
