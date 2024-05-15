using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class AccountStatusMV
    {

        public int AccountStatusID { get; set; }
        [Required(ErrorMessage="*Obavezno ispuniti")]
        [Display(Name ="Account Status")]
        public string AccountStatus { get; set; }
    }
}