using Contest.Shared;
using Contest.Shared.Enums;
using Contest.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContestServer.Services
{
    public class InMemoryContestantService : IContestantService
    {
        private ConcurrentDictionary<string, Contestant> contestants;
        private readonly ITimeService timeService;

        public IGameService GameService { get; }

        public InMemoryContestantService(IGameService gameService, ITimeService timeService)
        {
            contestants = new ConcurrentDictionary<string, Contestant>();
            GameService = gameService;
            this.timeService = timeService;
        }

        public void AddContestant(Contestant contestant)
        {
            if (contestant is null)
            {
                throw new ArgumentNullException(nameof(contestant));
            }
            if (nameIsDuplicate(contestant))
            {
                throw new ArgumentException("cannot add contestant with existing name");
            }

            contestants.TryAdd(contestant.Token, contestant);
        }

        private bool nameIsDuplicate(Contestant contestant)
        {
            var countNames = contestants.Values.Where(c => c.Name == contestant.Name).Count();
            return countNames > 0;
        }

        public bool ContestantExists(string token)
        {
            var tokenCount = contestants.Values.Where(c => c.Token == token).Count();
            return tokenCount > 0;
        }

        public Contestant GetContestantByToken(string token)
        {
            return contestants.Values.First(c => c.Token == token);
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

        public void UpdateContestant(Contestant contestant)
        {
            if (contestant is null)
            {
                throw new ArgumentNullException(nameof(contestant));
            }
            if(GameService.GetGameStatus().IsStarted == false)
                return;

            var existingContestantRecord = GetContestantByToken(contestant.Token);
            if (existingContestantRecord.CorrectFinalBoard != null)
                return;

            contestant = updateContestantIfFinished(contestant);

            contestants.AddOrUpdate(contestant.Token, contestant, (token, existing) => contestant);
            // Console.WriteLine("Contestants: " + JsonSerializer.Serialize(GetContestants()));
        }

        private Contestant updateContestantIfFinished(Contestant contestant)
        {
            if (contestant.GenerationsComputed == GameService.GetNumGenerations()
                && contestant.EndedGameAt == null)
            {
                contestant.EndedGameAt = timeService.Now();
                contestant = checkContestantBoard(contestant);
            }

            return contestant;
        }

        private Contestant checkContestantBoard(Contestant contestant)
        {
            if(contestant.FinalBoard == null)
                throw new ArgumentNullException("Final Board Cannot be null at last generation");

            var correctAnswer = GameService.CheckBoard(contestant.FinalBoard);
            Console.WriteLine($"Contestant {contestant.Name} computed board: " + correctAnswer);
            contestant.CorrectFinalBoard = correctAnswer;
            return contestant;
        }
    }
}
