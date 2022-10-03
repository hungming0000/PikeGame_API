using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Models
{
    public class SysUserDataModel
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string SysOrganizationID { get; set; }
        public string WorkStation { get; set; }
        public string SysGroupID { get; set; }
        public string SubOrganizationName { get; set; }
        public string UserName { get; set; }
        public bool? IsEnable { get; set; }
        public bool? IsDirector { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string CellPhone { get; set; }
        public string Memo { get; set; }
        public DateTime? LastResetPwTime { get; set; }
        public string ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }

        public IEnumerable<PermitModel> SysFunction { get; set; }
        public IEnumerable<UserTypeModel> UserType { get; set; }
    }
}
