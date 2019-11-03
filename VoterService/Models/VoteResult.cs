using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoterService.Models
{
    public class VoteResult
    {
        public int vote { get; set; }
        public int not_vote { get; set; }
        public int registerdVoter { get; set; }
    }
}
