using Contest.Shared;
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

        public RegisterController(IContestantService contestantService)
        {
            this.contestantService = contestantService ?? throw new ArgumentNullException(nameof(contestantService));
        }

        [HttpPost]
        public RegisterResponse Post([FromBody]RegisterRequest request)
        {
            var response = new RegisterResponse();
            response.Name = request.Name;
            response.Token = Guid.NewGuid().ToString();

            contestantService.AddContestant(new Contestant(response.Name, response.Token));

            return response;
        }
    }
}
