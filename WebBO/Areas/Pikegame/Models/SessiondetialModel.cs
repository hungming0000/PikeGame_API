using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Areas.Pikegame.Models
{
    public class SessiondetialModel
    {
        public int? sessiondetialid { get; set; }
        public int? sessionid { get; set; }
        public DateTime? sessiondetialtime { get; set; }
        public bool redhit { get; set; }
        public bool bluehit { get; set; }
        public int? redfraction { get; set; }
        public int? bluefraction { get; set; }
        public int? mstatus { get; set; }
        public int? finalredfraction { get; set; }
        public int? finalbluefraction { get; set; }
        public string bluehitposition { get; set; }
        public string redhitposition { get; set; }
    }
}
