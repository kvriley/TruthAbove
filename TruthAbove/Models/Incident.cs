using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruthAbove.Models
{
    public class UFOSighting
    {
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime IncidentDate { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Shape { get; set; }
        public string Duration { get; set; }
        public string Summary { get; set; }
        public DateTime PostedDate { get; set; }
    }
}