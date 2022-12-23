using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Areas.Pikegame.Models
{
    public class AdvertisesettingModel
    {
        public int advertiseid { get; set; }
        public string advertiseurl { get; set; }
        public string  adsstatus { get; set; }
        public string adsstatusName { get; set; }
        public DateTime advertisstarttime { get; set; }
        public DateTime advertisendtime { get; set; }
        public string advertistimeperiod { get; set; }
        public int advertiscosts { get; set; }
        public DateTime modifydate { get; set; }
        public string modifyuser { get; set; }
        public int periodtimes { get; set; }
    }
}
