using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Models
{
    public class PermitModel
    {
        public string SysPermitID { get; set; }
        public string SysGroupOrUserID { get; set; }
        public string SysFunctionID { get; set; }
        public string ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }

    }
}
