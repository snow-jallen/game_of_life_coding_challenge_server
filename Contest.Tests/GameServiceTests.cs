using Contest.Shared;
using Contest.Shared.Models;
using ContestServer.Services;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

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

        [Test]
        public void CheckBoardVerifysSolutionAccuracy()
        {
            var gameService = new GameService();
            gameService.endingBoard = new Coordinate[]
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(0, 1)
            };
            var submittedBoard = new Coordinate[]
            {
                new Coordinate(1, 2),
                new Coordinate(1, 1),
                new Coordinate(0, 1)
            };

            gameService.CheckBoard(submittedBoard).Should().BeTrue();
        }

        [Test]
        public void CheckBoardRejectsBadBoards()
        {
            var gameService = new GameService();
            gameService.endingBoard = new Coordinate[]
            {
                new Coordinate(1, 1),
                new Coordinate(1, 2),
                new Coordinate(0, 1)
            };
            var submittedBoard = new Coordinate[]
            {
                new Coordinate(1, 1),
                new Coordinate(0, 1)
            };

            gameService.CheckBoard(submittedBoard).Should().BeFalse();
        }

        [Test]
        public void CheckTheseTwoBoards()
        {
            var firstboard = JsonSerializer.Deserialize<IEnumerable<Coordinate>>("[{\"X\":16,\"Y\":-27},{\"X\":15,\"Y\":-27},{\"X\":13,\"Y\":-17},{\"X\":12,\"Y\":-16},{\"X\":14,\"Y\":-18},{\"X\":15,\"Y\":-18},{\"X\":16,\"Y\":-18},{\"X\":17,\"Y\":-17},{\"X\":12,\"Y\":-15},{\"X\":13,\"Y\":-13},{\"X\":15,\"Y\":-14},{\"X\":14,\"Y\":-12},{\"X\":15,\"Y\":-12},{\"X\":15,\"Y\":-11},{\"X\":16,\"Y\":-12},{\"X\":17,\"Y\":-13},{\"X\":18,\"Y\":-15},{\"X\":18,\"Y\":-16},{\"X\":11,\"Y\":-5},{\"X\":9,\"Y\":-5},{\"X\":9,\"Y\":-4},{\"X\":10,\"Y\":-3},{\"X\":10,\"Y\":-4},{\"X\":3,\"Y\":2},{\"X\":1,\"Y\":3},{\"X\":2,\"Y\":3},{\"X\":2,\"Y\":4},{\"X\":3,\"Y\":4},{\"X\":-4,\"Y\":10},{\"X\":-6,\"Y\":10},{\"X\":-6,\"Y\":11},{\"X\":-5,\"Y\":12},{\"X\":-5,\"Y\":11},{\"X\":-12,\"Y\":17},{\"X\":-14,\"Y\":18},{\"X\":-13,\"Y\":18},{\"X\":-13,\"Y\":19},{\"X\":-12,\"Y\":19},{\"X\":-19,\"Y\":25},{\"X\":-21,\"Y\":25},{\"X\":-21,\"Y\":26},{\"X\":-20,\"Y\":27},{\"X\":-20,\"Y\":26},{\"X\":16,\"Y\":-8},{\"X\":15,\"Y\":-6},{\"X\":15,\"Y\":-4},{\"X\":14,\"Y\":-4},{\"X\":16,\"Y\":-7},{\"X\":17,\"Y\":-7},{\"X\":17,\"Y\":-8},{\"X\":18,\"Y\":-8},{\"X\":18,\"Y\":-7},{\"X\":19,\"Y\":-6},{\"X\":20,\"Y\":-4},{\"X\":19,\"Y\":-4},{\"X\":18,\"Y\":6},{\"X\":17,\"Y\":6},{\"X\":15,\"Y\":-28},{\"X\":16,\"Y\":-28},{\"X\":17,\"Y\":7},{\"X\":18,\"Y\":7}]");
            var secondboard = JsonSerializer.Deserialize<IEnumerable<Coordinate>>("[{\"X\":16,\"Y\":-27},{\"X\":15,\"Y\":-27},{\"X\":13,\"Y\":-17},{\"X\":12,\"Y\":-16},{\"X\":14,\"Y\":-18},{\"X\":15,\"Y\":-18},{\"X\":16,\"Y\":-18},{\"X\":17,\"Y\":-17},{\"X\":12,\"Y\":-15},{\"X\":13,\"Y\":-13},{\"X\":15,\"Y\":-14},{\"X\":14,\"Y\":-12},{\"X\":15,\"Y\":-12},{\"X\":15,\"Y\":-11},{\"X\":16,\"Y\":-12},{\"X\":17,\"Y\":-13},{\"X\":18,\"Y\":-15},{\"X\":18,\"Y\":-16},{\"X\":11,\"Y\":-5},{\"X\":9,\"Y\":-5},{\"X\":9,\"Y\":-4},{\"X\":10,\"Y\":-3},{\"X\":10,\"Y\":-4},{\"X\":3,\"Y\":2},{\"X\":1,\"Y\":3},{\"X\":2,\"Y\":3},{\"X\":2,\"Y\":4},{\"X\":3,\"Y\":4},{\"X\":-4,\"Y\":10},{\"X\":-6,\"Y\":10},{\"X\":-6,\"Y\":11},{\"X\":-5,\"Y\":12},{\"X\":-5,\"Y\":11},{\"X\":-12,\"Y\":17},{\"X\":-14,\"Y\":18},{\"X\":-13,\"Y\":18},{\"X\":-13,\"Y\":19},{\"X\":-12,\"Y\":19},{\"X\":-19,\"Y\":25},{\"X\":-21,\"Y\":25},{\"X\":-21,\"Y\":26},{\"X\":-20,\"Y\":27},{\"X\":-20,\"Y\":26},{\"X\":16,\"Y\":-8},{\"X\":15,\"Y\":-6},{\"X\":15,\"Y\":-4},{\"X\":14,\"Y\":-4},{\"X\":16,\"Y\":-7},{\"X\":17,\"Y\":-7},{\"X\":17,\"Y\":-8},{\"X\":18,\"Y\":-8},{\"X\":18,\"Y\":-7},{\"X\":19,\"Y\":-6},{\"X\":20,\"Y\":-4},{\"X\":19,\"Y\":-4},{\"X\":18,\"Y\":6},{\"X\":17,\"Y\":6},{\"X\":15,\"Y\":-28},{\"X\":16,\"Y\":-28},{\"X\":17,\"Y\":7},{\"X\":18,\"Y\":7}]");
            
            var gameService = new GameService();
            gameService.endingBoard = secondboard;

            gameService.CheckBoard(firstboard).Should().BeTrue();
        }


    }
}
