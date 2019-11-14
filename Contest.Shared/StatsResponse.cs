using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest.Shared
{
    public class StatsResponse
    {
        public IEnumerable<Contestant> Contestants { get; set; }
    }
}
