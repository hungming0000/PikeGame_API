using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Models
{
    public class AppDownLoadModel
    {
        public string VersionID { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public string Describe { get; set; }
        public string DownloadUrl { get; set; }
    }
}
