using Contest.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestServer.Services
{
    public interface IContestantService
    {
        void AddContestant(Contestant contestant);
        IEnumerable<Contestant> GetContestants();
        Contestant GetContestant(string token);
    }
}
