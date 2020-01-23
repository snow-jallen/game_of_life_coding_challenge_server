using Contest.Shared;
using Contest.Shared.Enums;
using Contest.Shared.Http;
using Contest.Shared.Models;
using ContestServer.Controllers;
using ContestServer.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;

namespace Contest.Tests
{
    public class UpdateControllerTests
    {
        private IContestantService contestantService;
        private UpdateController updateController;
        private Mock<ITimeService> timeServiceMock;
        private Contestant contestant1;
        private DateTime time1 = new DateTime(2019, 11, 26, 20, 29, 1);
        private DateTime time2 = new DateTime(2019, 11, 26, 20, 29, 6);
        private DateTime time2a = new DateTime(2019, 11, 26, 20, 29, 7);
        private DateTime time3 = new DateTime(2019, 11, 26, 20, 29,11);

        [SetUp]
        public void Setup()
        {
            contestantService = new InMemoryContestantService();
            timeServiceMock = new Mock<ITimeService>();
            timeServiceMock.Setup(m => m.Now()).Returns(time1);

            contestant1 = new Contestant("Contestant1", "1234", timeServiceMock.Object.Now(), 0, ClientStatus.Waiting);
            contestantService.AddContestant(contestant1);

            updateController = new UpdateController(contestantService, timeServiceMock.Object, new GameService());
        }

        [Test]
        public void EmptyRequestFails()
        {
            var response = updateController.Post(null);
            response.IsError.Should().BeTrue();
            response.ErrorMessage.Should().Be("Invalid request");
        }

        [Test]
        public void UnrecognizedContestantFails()
        {
            var response = updateController.Post(new UpdateRequest{Token = "bogus"});
            response.IsError.Should().BeTrue();
            response.ErrorMessage.Should().Contain("not a registered player");
        }

        // [Test]
        // public void TooFrequentRequestFails()
        // {
        //     timeServiceMock.Setup(m => m.Now()).Returns(time2);

        //     var response1 = updateController.Post(new UpdateRequest { Token = "1234" });
        //     response1.IsError.Should().BeFalse("first update is ok");

        //     timeServiceMock.Setup(m => m.Now()).Returns(time2a);
        //     var response2 = updateController.Post(new UpdateRequest { Token = "1234" });
        //     response2.IsError.Should().BeTrue("another call to update too soon after the first would violate the rate limit.");
        //     response2.ErrorMessage.Should().Contain("rate limit");
        // }
    }
}