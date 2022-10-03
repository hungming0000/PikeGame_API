using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBO.Models;

namespace WebBO.DataClass
{
    public class CreateDTRequest
    {
        public string MainID { get; set; }
        public string TableName { get; set; }
        public List<SearchParams> Searchs { get; set; }

        public string Org { get; set; }

        public string OtherSql { get; set; }
    }
}
