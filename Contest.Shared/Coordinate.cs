using System;
using System.Text.Json.Serialization;

namespace Contest.Shared
{
    //[JsonConverter(typeof(CoordinateConverter))]
    public class Coordinate
    {

        public Coordinate() { }

        public Coordinate(int x, int y)
        {
            if (x == 0 || y == 0)
                throw new ArgumentOutOfRangeException("x or y", "0 values are invalid.  Must be positive or negative.");

            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"{{{X},{Y}}}";
        }

        public static Coordinate FromString(string serialized)
        {
            if (String.IsNullOrWhiteSpace(serialized))
                throw new ArgumentNullException();

            if (serialized.Length < 5)
                throw new ArgumentOutOfRangeException(nameof(serialized), "Must be at least 5");
            if(serialized[0] != '{' || serialized[serialized.Length-1] != '}')
                throw new ArgumentOutOfRangeException(nameof(serialized), "Must start with and end with {,}");
            var nums = serialized.Replace("{", String.Empty)
                .Replace("}", String.Empty)
                .Split(',');
            if (nums.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(serialized), "Must have two numbers separated by ,");

            if(int.TryParse(nums[0], out int x) && int.TryParse(nums[1], out int y))
            {
                return new Coordinate(x, y);
            }
            throw new ArgumentOutOfRangeException(nameof(serialized), "Unable to parse x and y");
        }
    }
}