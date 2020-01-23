using Contest.Shared;
using Contest.Shared.Enums;
using Contest.Shared.Models;
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
            if (contestant is null)
            {
                throw new ArgumentNullException(nameof(contestant));
            }

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

        public void RemoveContestant(Contestant contestant)
        {
            if (contestant is null)
            {
                throw new ArgumentNullException(nameof(contestant));
            }

            contestants.TryRemove(contestant.Token, out var removed);
        }

        public void UpdateContestant(Contestant contestant, ContestantStatus status)
        {
            if (contestant is null)
            {
                throw new ArgumentNullException(nameof(contestant));
            }

            if (status is null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            var startedGameAt = contestant.StartedGameAt;
            if (contestant.StatusCode == ClientStatus.Waiting && status.StatusCode == ClientStatus.Processing)
                startedGameAt = DateTime.Now;

            var endedGameAt = contestant.EndedGameAt;
            if (contestant.StatusCode == ClientStatus.Processing && status.StatusCode == ClientStatus.Complete)
                endedGameAt = DateTime.Now;

            var updatedContestant = new Contestant(contestant.Name,
                contestant.Token,
                status.LastSeen,
                status.GenerationsComputed,
                status.StatusCode,
                startedGameAt,
                endedGameAt);

            contestants.AddOrUpdate(contestant.Token, updatedContestant, (token, existing) => updatedContestant);
        }
    }
}
