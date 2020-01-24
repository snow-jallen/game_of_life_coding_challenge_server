using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Contest.Shared;
using Contest.Shared.Enums;
using Contest.Shared.Http;
using Contest.Shared.Models;
using ContestServer.Services;
using Microsoft.AspNetCore.Mvc;


namespace ContestServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdateController : ControllerBase
    {
        private readonly IContestantService contestantService;
        private readonly ITimeService timeService;
        private readonly GameService gameService;
        public const int UpdateRateLimitInSeconds = 1;

        public UpdateController(IContestantService contestantService, ITimeService timeService, GameService gameService)
        {
            this.contestantService = contestantService ?? throw new ArgumentNullException(nameof(contestantService));
            this.timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            this.gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        [HttpPost]
        public UpdateResponse Post([FromBody]UpdateRequest request)
        {
            Console.WriteLine("Recieved Request: " + JsonSerializer.Serialize(request));
            var response = new UpdateResponse();
            if (request == null)
            {
                response.IsError = true;
                response.ErrorMessage = "Invalid request";
                return response;
            }

            var contestant = contestantService.GetContestant(request.Token);

            if(contestant == null)
            {
                response.IsError = true;
                response.ErrorMessage = "You are not a registered player.  Register to play at /register";
                return response;
            }

            var timeSinceLastUpdate = timeService.Now() - contestant.LastSeen;
            if(timeSinceLastUpdate.TotalSeconds < UpdateRateLimitInSeconds)
            {
                response.IsError = true;
                response.ErrorMessage = $"The rate limit requires you call this endpoint no more than once every {UpdateRateLimitInSeconds} seconds.";
                return response;
            }

            contestantService.UpdateContestant(contestant, new ContestantStatus
            {
                LastSeen = timeService.Now(),
                GenerationsComputed = request.GenerationsComputed,
                StatusCode = request.Status
            });

            var gameStatus = gameService.GetGameStatus();

            response.GameState = gameStatus.IsStarted ? GameState.InProgress : GameState.NotStarted;
            if (gameStatus.IsGameOver)
                response.GameState = GameState.Over;

            if(gameStatus.IsStarted)
            {
                response.SeedBoard = gameStatus.SeedBoard;
                response.GenerationsToCompute = gameStatus.NumberGenerations;
            }

            response.IsError = false;

            return response;
        }
    }
}
