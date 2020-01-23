using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contest.Shared.Models;

namespace Contest.Shared.Http
{
    public class StatsResponse
    {
        public IEnumerable<Contestant> Contestants { get; set; }
    }
}
