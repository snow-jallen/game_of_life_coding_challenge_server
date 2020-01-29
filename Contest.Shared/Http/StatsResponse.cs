using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contest.Shared.Models;

namespace Contest.Shared.Http
{
    public class StatsResponse
    {
        public StatsResponse(IEnumerable<Contestant> contestants)
        {
            Contestants = contestants.Select(c => {
                return new Contestant(
                    c.Name,
                    "",
                    c.LastSeen,
                    c.GenerationsComputed,
                    c.StartedGameAt,
                    c.EndedGameAt,
                    new Coordinate[]{},
                    c.CorrectFinalBoard
                );
            });
        }
        public IEnumerable<Contestant> Contestants { get; }
    }
}
