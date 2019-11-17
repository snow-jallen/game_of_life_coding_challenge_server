using System;

namespace Contest.Shared
{
    public class Contestant
    {
        public Contestant(string name, string token, DateTime lastSeen)
        {
            Name = name;
            Token = token;
            LastSeen = lastSeen;
        }

        public string Name { get; }
        public string Token { get; }
        public DateTime LastSeen { get; }
    }
}