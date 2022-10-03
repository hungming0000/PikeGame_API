using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Areas.BusStopManagement.Models
{
    public class BusshelterModel
    {
        public string id { get; set; }
        public string district { get; set; }
        public string busstopname { get; set; }
        public string location { get; set; }
        public string status { get; set; }
        public string suggestunit { get; set; }
        public string types { get; set; }
        public string undertaker { get; set; }
        public string note { get; set; }
        public string taipowernum { get; set; }
        public string dir { get; set; }
        public string handlestat { get; set; }
        public string movestat { get; set; }
        public int? stationnum { get; set; }
        public string villname { get; set; }
        public int? setyears { get; set; }
        public string propertynum { get; set; }
        public DateTime? registeryear { get; set; }
    }
}
