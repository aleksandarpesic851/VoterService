using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoterService.Models
{
    public class VoterModel
    {
        [Key]
        public int id { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public string nic { get; set; }
        public string address { get; set; }
        public int district { get; set; }
        public int province { get; set; }
        public int vote_state { get; set; }
    }
}
