using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class RequestTypeMV
    {
        
        public int RequestTypeID { get; set; }
        [Required(ErrorMessage="*Obavezno ispuniti")]
        [Display(Name = "Request Type")]
        public string RequestType { get; set; }
    }
}