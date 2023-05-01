using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Metrics;
using System.Web.Mvc;

namespace jobfinder.Models
{
    public class country
    {
        public int countryid { get; set; }
        public string cname { get; set; }

       // public ICollection<state> states { get; set;}
       //public string countrycode { get; set; }
        public List<SelectListItem> Countries { get; set; }

    }
}
