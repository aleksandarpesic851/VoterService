using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoterService.Models
{
    public class VoterSearchModel
    {
        //All Districts id array, it's used for search db
        public List<int> arrDistricts { get; set; }
        public string userID { get; set; }
    }
}
