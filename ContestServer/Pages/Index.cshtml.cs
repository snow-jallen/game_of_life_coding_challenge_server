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
        private readonly ITimeService timeService;
        public const int UserExpirationInSeconds = 30;

        public IndexModel(IContestantService contestantService, ITimeService timeService)
        {
            this.contestantService = contestantService ?? throw new ArgumentNullException(nameof(contestantService));
            this.timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        public IEnumerable<Contestant> Contestants { get; private set; }

        public void OnGet()
        {
            var allContestants = contestantService.GetContestants();
            var staleContestants = from c in allContestants
                                   where c.LastSeen.AddSeconds(UserExpirationInSeconds) < timeService.Now()
                                   select c;

            foreach ( var staleContestant in staleContestants)
            {
                contestantService.RemoveContestant(staleContestant);
            }

            Contestants = allContestants.Except(staleContestants);
        }
    }
}
