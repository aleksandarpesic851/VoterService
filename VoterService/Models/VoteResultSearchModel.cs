using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoterService.Models
{
    public class VoteResultSearchModel
    {
        public List<int> arrDistricts { get; set; }
        public int party { get; set; }

    }
}
