using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class UserMV
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UsernameOrEmail { get; set; }
        public int AccountStatusID { get; set; }
        public string AccountStatus { get; set; }
        public int UserTypeID { get; set; }
        public string UserType { get; set; }
    }
}