using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Metrics;
using System.Web.Mvc;

namespace jobfinder.Models
{
    public class city
    {
        public int cityid { get; set; }

        public string cname { get; set; }

       public int stateid { get; set; }

        public state state { get; set; }

        public List<SelectListItem> Cities { get; set; }
    }
}
