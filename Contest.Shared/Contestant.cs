using System;

namespace Contest.Shared
{
    public class Contestant
    {
        public Contestant(string name, string token)
        {
            Name = name;
            Token = token;
        }

        public string Name { get; }
        public string Token { get; }
    }
}