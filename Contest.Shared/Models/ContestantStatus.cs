using System;
using System.Collections.Generic;
using System.Text;
using Contest.Shared.Enums;

namespace Contest.Shared.Models
{
    public class ContestantStatus
    {
        public long? GenerationsComputed { get; set; }
        public ClientStatus StatusCode { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
