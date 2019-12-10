using ContestServer;
using ContestServer.Services;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Contest.Tests
{
    public class StartGameTests
    {
        [Test]
        public void CannotParseSeedBoard()
        {
            //var gameService = new GameService();
            //var startGamePageModel = new StartGameModel(gameService);
            //startGamePageModel.SeedBoard = "aoeu";

            //startGamePageModel
            //    .Invoking(m => m.OnPost())
            //    .Should()
            //    .Throw<JsonException>("Can't parse board");
        }
    }
}
