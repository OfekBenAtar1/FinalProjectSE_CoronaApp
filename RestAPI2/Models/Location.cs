using System;

namespace CoronaProgram.Models
{
    public class Location
    {
        public DateTime DateOfVisit { get; set; }
        public string SiteName { get; set; }
        public Address SiteAddress { get; set; }

    }
}