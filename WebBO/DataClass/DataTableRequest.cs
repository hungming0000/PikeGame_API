using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBO.Models;

namespace WebBO.DataClass
{
    public class DataTableRequest
    {
        public string UserID { get; set; }

        public string SysOrganizationID { get; set; }

        public DTParameters Parameters { get; set; }

        public List<SearchParams> Searchs { get; set; }

        public string KeyWord { get; set; }
    }
}
