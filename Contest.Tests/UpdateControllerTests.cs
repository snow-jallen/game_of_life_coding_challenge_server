using Contest.Shared;
using ContestServer.Controllers;
using ContestServer.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Contest.Tests
{
    public class UpdateControllerTests
    {
        private UpdateController updateController;

        [SetUp]
        public void Setup()
        {
            var contestantServiceMock = new Mock<IContestantService>();


            updateController = new UpdateController(contestantServiceMock.Object);
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
    }
}