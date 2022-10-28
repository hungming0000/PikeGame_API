using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Areas.Pikegame.Models
{
    public class TournamentDetailModel
    {
        public int tournamentid { get; set; }
        public string tournamentname { get; set; }
        public string tournamentdate { get; set; }
        public DateTime? tournamentstartdate { get; set; }
        public DateTime? tournamentenddate { get; set; }
        public Int16? maxfraction { get; set; }
        public SessionModel Item { get; set; }

    }

    public class SessionModel
    {
        public int sessionid { get; set; }
        public int tournamentid { get; set; }
        public string judge_account { get; set; }
        public string sessionname { get; set; }
        public DateTime? sessiontime { get; set; }
        public string red_accountid { get; set; }
        public string red_account { get; set; }
        public string blue_accountid { get; set; }
        public string blue_account { get; set; }
        public Int16? redfraction_sum { get; set; }
        public Int16? bluefraction_sum { get; set; }
        public bool mstatus { get; set; }

    }
}
