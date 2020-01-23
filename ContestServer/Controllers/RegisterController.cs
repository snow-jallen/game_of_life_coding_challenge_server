using Contest.Shared;
using Contest.Shared.Enums;
using Contest.Shared.Http;
using Contest.Shared.Models;
using ContestServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IContestantService contestantService;
        private readonly ITimeService timeService;

        public RegisterController(IContestantService contestantService, ITimeService timeService)
        {
            this.contestantService = contestantService ?? throw new ArgumentNullException(nameof(contestantService));
            this.timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        [HttpPost]
        public RegisterResponse Post([FromBody]RegisterRequest request)
        {
            var response = new RegisterResponse();
            response.Name = request.Name;
            response.Token = Guid.NewGuid().ToString();

            contestantService.AddContestant(new Contestant(response.Name, response.Token, timeService.Now(), 0, ClientStatus.Waiting));

            return response;
        }
    }
}
