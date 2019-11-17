using Contest.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestServer.Services
{
    public class InMemoryContestantService : IContestantService
    {
        private ConcurrentDictionary<string, Contestant> contestants;

        public InMemoryContestantService()
        {
            contestants = new ConcurrentDictionary<string, Contestant>();
        }

        public void AddContestant(Contestant contestant)
        {
            contestants.TryAdd(contestant.Token, contestant);
        }

        public Contestant GetContestant(string token)
        {
            return contestants.GetValueOrDefault(token);
        }

        public IEnumerable<Contestant> GetContestants()
        {
            return contestants.Values.ToArray();
        }

        public void UpdateContestantLastSeen(Contestant contestant, DateTime newLastSeen)
        {
            var updatedContestant = new Contestant(contestant.Name, contestant.Token, newLastSeen);

            contestants.AddOrUpdate(contestant.Token, updatedContestant, (token, existing) => updatedContestant);
        }
    }
}
