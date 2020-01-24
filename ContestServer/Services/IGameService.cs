using System.Collections.Generic;
using Contest.Shared.Models;

namespace ContestServer.Services
{
    public interface IGameService
    {
        bool CheckBoard(IEnumerable<Coordinate> submittedBoard);
        void EndGame();
        GameStatus GetGameStatus();
        void StartGame(IEnumerable<Coordinate> seed, long numGenerations, IEnumerable<Coordinate> endingBoard);
        long GetNumGenerations();
    }
}