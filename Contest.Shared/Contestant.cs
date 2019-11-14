using System;

namespace Contest.Shared
{
    public class Contestant
    {
        public Contestant(string name, Guid token)
        {
            Name = name;
            Token = token;
        }

        public string Name { get; }
        public Guid Token { get; }
    }
}