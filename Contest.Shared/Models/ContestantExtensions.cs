using System;
using System.Collections.Generic;

namespace Contest.Shared.Models
{
    public static class ContestantExtensions
    {
        public static Contestant UpdateGenerationsComputed(this Contestant contestant, long? generationsComputed)
        {
            return new Contestant(
                contestant.Name,
                contestant.Token,
                contestant.LastSeen,
                generationsComputed,
                contestant.StartedGameAt,
                contestant.EndedGameAt,
                contestant.FinalBoard,
                contestant.CorrectFinalBoard
            );
        }
        public static Contestant UpdateFinalBoard(this Contestant contestant, IEnumerable<Coordinate> finalBoard)
        {
            return new Contestant(
                contestant.Name,
                contestant.Token,
                contestant.LastSeen,
                contestant.GenerationsComputed,
                contestant.StartedGameAt,
                contestant.EndedGameAt,
                finalBoard,
                contestant.CorrectFinalBoard
            );
        }
        public static Contestant UpdateStartedGameAt(this Contestant contestant, DateTime startedTime)
        {
            return new Contestant(
                contestant.Name,
                contestant.Token,
                contestant.LastSeen,
                contestant.GenerationsComputed,
                startedTime,
                contestant.EndedGameAt,
                contestant.FinalBoard,
                contestant.CorrectFinalBoard
            );
        }
        public static Contestant UpdateEndedGameAt(this Contestant contestant, DateTime endedAt)
        {
            return new Contestant(
                contestant.Name,
                contestant.Token,
                contestant.LastSeen,
                contestant.GenerationsComputed,
                contestant.StartedGameAt,
                endedAt,
                contestant.FinalBoard,
                contestant.CorrectFinalBoard
            );
        }
        public static Contestant UpdateCorrectFinalBoard(this Contestant contestant, bool? correctFinalBoard)
        {
            return new Contestant(
                contestant.Name,
                contestant.Token,
                contestant.LastSeen,
                contestant.GenerationsComputed,
                contestant.StartedGameAt,
                contestant.EndedGameAt,
                contestant.FinalBoard,
                correctFinalBoard
            );
        }
    }   
}