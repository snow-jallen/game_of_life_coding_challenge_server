using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest.Shared
{
    public class UpdateRequest
    {
        public string Token { get; set; }
        public ClientStatus Status { get; set; }
        public int? GenerationsComputed { get; set; }
        public IEnumerable<Coordinate> ResultBoard { get; set; }
    }
}
