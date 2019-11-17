using System;

namespace Contest.Shared
{
    public class Coordinate
    {
        public Coordinate(int x, int y)
        {
            if (x == 0 || y == 0)
                throw new ArgumentOutOfRangeException("x or y", "0 values are invalid.  Must be positive or negative.");

            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}