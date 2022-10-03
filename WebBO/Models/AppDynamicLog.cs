using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Models
{
    public class AppDynamicLog
    {
        public string AppDynamicLogID { get; set; }
        public string ContractID { get; set; }
        public DateTime LogDate { get; set; }
        public string LogNo { get; set; }
        public string LogContent { get; set; }
        public string Memo { get; set; }
        public string ModifyUser { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
