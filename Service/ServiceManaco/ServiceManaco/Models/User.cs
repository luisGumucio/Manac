using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceManaco.Models
{
    public class User
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Token { get; set; }
    }
}