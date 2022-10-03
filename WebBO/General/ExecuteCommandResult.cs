using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using isRock.Framework;
using isRock.Framework.WebAPI;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

namespace WebBO.General
{
    public class ExecuteCommandAPIResult<T> : ExecuteCommandDefaultResult<T>
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public int Count { get; set; }
    }

    public class ExecuteCommandAPIResult : ExecuteCommandDefaultResult
    {
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public int Count { get; set; }
    }
    public class GetResult
    {
        public string Error { set; get; }
        public string Result { set; get; }
        public string ID { set; get; }
        public string Other { set; get; }
    }
}