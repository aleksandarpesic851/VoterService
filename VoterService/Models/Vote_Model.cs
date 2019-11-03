using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoterService.Models
{
    public class Vote_Model
    {
        public string userid { get; set; }
        public int district { get; set; }
        public int state { get; set; }
    }
}
