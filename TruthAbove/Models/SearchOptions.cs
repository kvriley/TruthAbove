using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TruthAbove.Models
{
    public class SearchOptions
    {
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Shapes { get; set; }
    }
}