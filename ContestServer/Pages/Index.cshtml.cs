using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contest.Shared;
using ContestServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContestServer
{
    public class IndexModel : PageModel
    {
        private readonly IContestantService contestantService;

        public IndexModel(IContestantService contestantService)
        {
            this.contestantService = contestantService ?? throw new ArgumentNullException(nameof(contestantService));
        }

        public IEnumerable<Contestant> Contestants { get; private set; }

        public void OnGet()
        {
            Contestants = contestantService.GetContestants();
        }
    }
}
