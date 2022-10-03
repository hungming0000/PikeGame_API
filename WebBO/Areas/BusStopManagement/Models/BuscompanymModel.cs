using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Areas.BusStopManagement.Models
{
    public class BuscompanymModel
    {
        public string bcm_buscompanyno { get; set; }
        public string bcm_chinesename { get; set; }
        public string bcm_englishname { get; set; }
        public string bcm_cityno { get; set; }
        public string city { get; set; }
        public string bcm_address { get; set; }
        public string bcm_phone { get; set; }
        public string bcm_fax { get; set; }
        public double bcm_longitude { get; set; }
        public double bcm_latitude { get; set; }
        public string bcm_website { get; set; }
        public int status { get; set; }
        public string statustxt { get; set; }
        public string bcm_mail { get; set; }
    }
}
