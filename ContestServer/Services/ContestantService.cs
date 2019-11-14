using Contest.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestServer.Services
{
    public class ContestantService : IContestantService
    {
        private System.Collections.Concurrent.ConcurrentBag<Contestant> contestants;

        public ContestantService()
        {
            contestants = new System.Collections.Concurrent.ConcurrentBag<Contestant>();
        }

        public void AddContestant(Contestant contestant)
        {
            contestants.Add(contestant);
        }

        public IEnumerable<Contestant> GetContestants()
        {
            return contestants.ToArray();
        }
    }
}
