using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Areas.Pikegame.Models
{
    public class VideosesettingModel
    {
        public int videoid { get; set; }
        public string videourl { get; set; }
        public string videostatus { get; set; }
        public string videotitle { get; set; }
        public string videodescription { get; set; }
        public DateTime modifydate { get; set; }
        public string modifyuser { get; set; }
    }
}
