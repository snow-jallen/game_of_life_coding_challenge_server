using System;
using System.Collections.Generic;
using System.Text;

namespace Contest.Shared
{
    public class ContestantStatus
    {
        public long? GenerationsComputed { get; set; }
        public ClientStatus StatusCode { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
