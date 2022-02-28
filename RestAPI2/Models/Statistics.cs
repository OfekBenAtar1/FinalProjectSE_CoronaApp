using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaProgram.Models
{
    public class Statistics
    {
        public int Infected { get; set; }
        public int Healed { get; set; }
        public int Isolated { get; set; }
        public List<CityStatistics> CityStatistics {get;set;}
    }
}
