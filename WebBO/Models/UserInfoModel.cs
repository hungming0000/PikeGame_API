using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Models
{
    public class UserInfoModel
    {
        public string UserID { get; set; }
        public bool? IsDirector { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string CellPhone { get; set; }
        public string Memo { get; set; }
    }
}
