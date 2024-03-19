using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class FinderMV
    {
        public FinderMV()
        {
            SearchResult = new List<FinderSearchResultsMV>();
        }
        public int BloodGroupID { get; set; }
        public int CityID { get; set; }
        public List<FinderSearchResultsMV> SearchResult { get; set; }

    }
}