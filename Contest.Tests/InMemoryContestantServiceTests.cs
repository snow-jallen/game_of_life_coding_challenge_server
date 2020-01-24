using System;
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

        [SetUp]
        public void SetUp()
        {
            gameServiceMoq = new Mock<IGameService>();
        }

        [Test]
        public void CannotAddDuplicateNames()
        {
            
            var contestantService = new InMemoryContestantService(gameServiceMoq.Object);

            var wednesday = new Contestant("wednesday", "some token", DateTime.Now, 0);
            var impostorWednesday = new Contestant("wednesday", "other token", DateTime.Now, 0);
            
            contestantService.AddContestant(wednesday);


            contestantService
                .Invoking(cs => cs.AddContestant(impostorWednesday))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("cannot add contestant with existing name");
        }

        //update contestant should trigger testing the submited board of the contestand and giving a pass fail grade
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
                
            var contestantService = new InMemoryContestantService(gameServiceMoq.Object);
            var wednesday = new Contestant(
                "wednesday",
                "wednesday's token",
                DateTime.Now,
                lastGeneration,
                DateTime.Now,
                DateTime.Now,
                finalBoard
            );
            contestantService.AddContestant(wednesday);

            contestantService.UpdateContestant(wednesday);

            var newWednesday = contestantService.GetContestantByToken(wednesday.Token);
            newWednesday.CorrectFinalBoard.Should().BeTrue();
        }
    }
}