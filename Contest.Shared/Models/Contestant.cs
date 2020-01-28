using System;
using System.Collections.Generic;
using Contest.Shared.Enums;

namespace Contest.Shared.Models
{
    public class Contestant
    {
        public Contestant()
        {
            
        }
        public Contestant(
            string name, 
            string token, 
            DateTime lastSeen, 
            long? generationsComputed, 
            DateTime? startedGameAt=null, 
            DateTime? endedGameAt=null,
            IEnumerable<Coordinate> finalBoard=null,
            bool? correctFinalBoard=null)
        {
            Name = name;
            Token = token;
            LastSeen = lastSeen;
            GenerationsComputed = generationsComputed;
            StartedGameAt = startedGameAt;
            EndedGameAt = endedGameAt;
            FinalBoard=finalBoard;
            CorrectFinalBoard = correctFinalBoard;
        }

        public string Name { get; set;  }
        public string Token { get; set;  }
        public DateTime LastSeen { get; set;  }
        public long? GenerationsComputed { get; set;  }
        public DateTime? StartedGameAt { get; set;  }
        public DateTime? EndedGameAt { get; set;  }
        public TimeSpan? Elapsed
        {
            get
            {
                if (StartedGameAt == null || EndedGameAt == null)
                    return null;
                return EndedGameAt - StartedGameAt;
            }
        }
        public IEnumerable<Coordinate> FinalBoard { get; set;  }
        public bool? CorrectFinalBoard { get; set;  }
    }
}