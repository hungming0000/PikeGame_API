using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Areas.Pikegame.Models
{
    public class TournamentModel
    {
        public string tournamentname { get; set; }
        public string tournamentstartdate { get; set; }
        public string tournamentenddate { get; set; }
        public int? maxfraction { get; set; }
        public int tournamentid { get; set; }
        public string accountid { get; set; }
        public string judge_accountid { get; set; }
        public string sessionname { get; set; }
        public string red_accountid { get; set; }
        public string blue_accountid { get; set; }
        public int? redfraction_sum { get; set; }
        public int? bluefraction_sum { get; set; }
        public bool mstatus { get; set; }

    }
}
