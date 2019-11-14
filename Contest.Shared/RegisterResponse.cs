using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest.Shared
{
    public class RegisterResponse
    {
        public Guid Token { get; set; }
        public string Name { get; set; }
    }
}
