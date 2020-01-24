using System;
using Contest.Shared.Enums;

namespace Contest.Shared.Models
{
    public class Contestant
    {
        public Contestant(
            string name, 
            string token, 
            DateTime lastSeen, 
            long? generationsComputed, 
            DateTime? startedGameAt=null, 
            DateTime? endedGameAt=null,
            bool? correctFinalBoard=false)
        {
            Name = name;
            Token = token;
            LastSeen = lastSeen;
            GenerationsComputed = generationsComputed;
            StartedGameAt = startedGameAt;
            EndedGameAt = endedGameAt;
            CorrectFinalBoard = correctFinalBoard;
        }

        public string Name { get; }
        public string Token { get; }
        public DateTime LastSeen { get; }
        public long? GenerationsComputed { get; }
        public DateTime? StartedGameAt { get; }
        public DateTime? EndedGameAt { get; }
        public TimeSpan? Elapsed
        {
            get
            {
                if (StartedGameAt == null || EndedGameAt == null)
                    return null;
                return EndedGameAt - StartedGameAt;
            }
        }
        public bool? CorrectFinalBoard { get; }
    }
}