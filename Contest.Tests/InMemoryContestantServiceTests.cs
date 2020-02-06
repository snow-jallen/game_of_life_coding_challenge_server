using System;
using System.Linq;
using Contest.Shared.Models;
using ContestServer.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Contest.Tests
{
    public class InMemoryContestantServiceTests
    {
        public Mock<IGameService> gameServiceMoq { get; private set; }

        private Mock<ITimeService> timeServiceMoq;
        private InMemoryContestantService contestantService;

        [SetUp]
        public void SetUp()
        {
            gameServiceMoq = new Mock<IGameService>();
            timeServiceMoq = new Mock<ITimeService>();
            contestantService = new InMemoryContestantService(gameServiceMoq.Object, timeServiceMoq.Object);
            timeServiceMoq.Setup(ts => ts.Now()).Returns(DateTime.Now);
        }

        [Test]
        public void CannotAddDuplicateNames()
        {
            var wednesday = new Contestant("wednesday", "some token", DateTime.Now, 0);
            var impostorWednesday = new Contestant("wednesday", "other token", DateTime.Now, 0);
            
            contestantService.AddContestant(wednesday);


            contestantService
                .Invoking(cs => cs.AddContestant(impostorWednesday))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("cannot add contestant with existing name");
        }

        [Test]
        public void ContestantWithCorrectBoardPasses()
        {
            var lastGeneration = 100;
            var finalBoard = new Coordinate[]
            {
                new Coordinate{ X = 1, Y = 1}
            };
            gameServiceMoq
                .Setup(gs => gs.GetNumGenerations())
                .Returns(lastGeneration);
            gameServiceMoq
                .Setup(gs => gs.CheckBoard(finalBoard))
                .Returns(true);
            gameServiceMoq
                .Setup(gs => gs.GetGameStatus())
                .Returns(new GameStatus(finalBoard));

            var wednesday = new Contestant(
                "wednesday",
                "wednesday's token",
                DateTime.Now,
                0
            );
            contestantService.AddContestant(wednesday);

            wednesday = wednesday
                        .UpdateGenerationsComputed(lastGeneration)
                        .UpdateFinalBoard(finalBoard);

            contestantService.UpdateContestant(wednesday);

            var newWednesday = contestantService.GetContestantByToken(wednesday.Token);
            newWednesday.CorrectFinalBoard.Should().BeTrue();
        }

        [Test]
        public void SubmitingFinalBoardAddsEndTimeToContestant()
        {
            var lastGeneration = 100;
            var finalBoard = new Coordinate[]
            {
                new Coordinate{ X = 1, Y = 1}
            };
            gameServiceMoq
                .Setup(gs => gs.GetNumGenerations())
                .Returns(lastGeneration);
            gameServiceMoq
                .Setup(gs => gs.GetGameStatus())
                .Returns(new GameStatus(finalBoard));
            
            var wednesday = new Contestant(
                "wednesday",
                "wednesday's token",
                DateTime.Now,
                0,
                DateTime.Now
            );
            contestantService.AddContestant(wednesday);

            wednesday = wednesday
                        .UpdateGenerationsComputed(lastGeneration)
                        .UpdateFinalBoard(finalBoard);

            contestantService.UpdateContestant(wednesday);

            wednesday = contestantService.GetContestantByToken(wednesday.Token);
            wednesday.Elapsed.Should().NotBeNull();
        }

        [Test]
        public void VerifyGradesArePersisted()
        {
            var gameService = new GameService();
            var contestantService = new InMemoryContestantService(gameService, timeServiceMoq.Object);
            var startingBoard = new Coordinate[]
            {
                new Coordinate{ X = 1, Y = 1}
            };
            var endingBoard = new Coordinate[]
            {
                new Coordinate{ X = 1, Y = 1}
            };

            gameService.StartGame(startingBoard, 100, endingBoard);
            var jonathan = new Contestant(
                "jonathan",
                "token",
                DateTime.Now,
                0
            );
            contestantService.AddContestant(jonathan);

            jonathan = jonathan
                        .UpdateGenerationsComputed(100)
                        .UpdateFinalBoard(endingBoard);

            contestantService.UpdateContestant(jonathan);

            var newJonathan = contestantService.GetContestantByToken(jonathan.Token);
            newJonathan.CorrectFinalBoard.Should().BeTrue();
        }

        [Test]
        public void CanResetContestantList()
        {
            var wednesday = new Contestant(
                "wednesday",
                "wednesday's token",
                DateTime.Now,
                0
            );
            contestantService.AddContestant(wednesday);

            contestantService.ResetContestantList();

            contestantService.GetContestants
            
        }
    }
}