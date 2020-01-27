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
                return new Contestant{
                    Name = c.Name,
                    GenerationsComputed = c.GenerationsComputed,
                    StartedGameAt = c.StartedGameAt,
                    EndedGameAt = c.EndedGameAt,
                    LastSeen = c.LastSeen,
                    CorrectFinalBoard = c.CorrectFinalBoard
                };
            });
        }
        public IEnumerable<Contestant> Contestants { get; }
    }
}
