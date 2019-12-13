using Contest.Shared;
using ContestServer.Services;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contest.Tests
{
    public class GameServiceTests
    {
        [Test]
        public void GameServiceStartsOffWithNoGame()
        {
            var gameService = new GameService();
            var gameStatus = gameService.GetGameStatus();
            gameStatus.IsStarted.Should().BeFalse();
        }

        [Test]
        public void StartingGameShowsThatGameHasStarted()
        {
            var gameService = new GameService();

            var seed = new[]
            {
                new Coordinate(1,1),
                new Coordinate(1,2)
            };
            gameService.StartGame(seed, numGenerations: 5, endingBoard: null);

            var gameStatus = gameService.GetGameStatus();
            gameStatus.IsStarted.Should().BeTrue();
        }


    }
}
