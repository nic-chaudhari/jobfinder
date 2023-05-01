using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Metrics;
using System.Web.Mvc;

namespace jobfinder.Models

{
    public class state
    {
        public int stateid { get; set; }
        public string sname { get; set; }

        public int countryid { get; set; }

        //public ICollection<city> cities { get; set; }
        public country country { get; set; }

      

        public List<SelectListItem> States { get; set; }


    }
}
