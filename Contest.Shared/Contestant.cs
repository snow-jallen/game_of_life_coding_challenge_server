using System;

namespace Contest.Shared
{
    public class Contestant
    {
        public Contestant(string name, string token, DateTime lastSeen, long? generationsComputed, ClientStatus statusCode)
        {
            Name = name;
            Token = token;
            LastSeen = lastSeen;
            GenerationsComputed = generationsComputed;
            StatusCode = statusCode;
        }

        public string Name { get; }
        public string Token { get; }
        public DateTime LastSeen { get; }
        public long? GenerationsComputed { get; }
        public ClientStatus StatusCode { get; }
    }
}