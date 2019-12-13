using Contest.Shared;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleClient
{
    public interface IContestServer
    {
        [Post("/register")]
        Task<RegisterResponse> Register(RegisterRequest request);

        [Post("/update")]
        Task<UpdateResponse> Update(UpdateRequest request);
    }
}
