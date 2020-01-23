using Contest.Shared;
using Contest.Shared.Http;
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
    public class StatsController : ControllerBase
    {
        private readonly IContestantService contestantService;

        public StatsController(IContestantService contestantService)
        {
            this.contestantService = contestantService ?? throw new ArgumentNullException(nameof(contestantService));
        }

        [HttpGet]
        public StatsResponse Get()
        {
            return new StatsResponse { Contestants = contestantService.GetContestants() };
        }
    }
}
