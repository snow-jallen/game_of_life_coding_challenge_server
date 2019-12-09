using Contest.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestServer.Services
{
    public class GameService
    {
        private GameStatus gameStatus;

        public GameService()
        {
            gameStatus = new GameStatus();
        }

        public GameStatus GetGameStatus() => gameStatus;

        public void StartGame(IEnumerable<Coordinate> seed, int numGenerations)
        {
            gameStatus = new GameStatus(seed, numGenerations);
        }
    }

    public class GameStatus
    {
        public GameStatus(IEnumerable<Coordinate> seedBoard = null, int? numGenerations = null)
        {
            IsStarted = seedBoard != null;
            SeedBoard = seedBoard;
            NumberGenerations = numGenerations;
        }
        public bool IsStarted { get; private set; }
        public IEnumerable<Coordinate> SeedBoard { get; private set; }
        public int? NumberGenerations { get; private set; }
    }
}
