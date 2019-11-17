using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contest.Shared;
using ContestServer.Services;
using Microsoft.AspNetCore.Mvc;


namespace ContestServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdateController : ControllerBase
    {
        private readonly IContestantService contestantService;

        public UpdateController(IContestantService contestantService)
        {
            this.contestantService = contestantService ?? throw new ArgumentNullException(nameof(contestantService));
        }

        [HttpPost]
        public UpdateResponse Post([FromBody]UpdateRequest request)
        {
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



            return response;
        }
    }
}
