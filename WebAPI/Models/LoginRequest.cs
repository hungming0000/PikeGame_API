using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class LoginRequest
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string LoginType { get; set; }
        public string Premises { get; set; }
    }
}