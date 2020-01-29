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
        public IEnumerable<Coordinate> FinalBoard { get; }
        public bool? CorrectFinalBoard { get; }

    }
}