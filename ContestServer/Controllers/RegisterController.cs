using Contest.Shared;
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
        public RegisterController()
        {

        }

        [HttpGet]
        public RegisterResponse Get([FromBody]RegisterRequest request)
        {
            var response = new RegisterResponse();
            response.Name = request.Name;
            response.Token = Guid.NewGuid();

            return response;
        }
    }
}
