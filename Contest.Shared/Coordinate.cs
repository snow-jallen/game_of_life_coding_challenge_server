using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Contest.Shared
{
    //[JsonConverter(typeof(CoordinateConverter))]
    public class Coordinate : IEquatable<Coordinate>
    {

        public Coordinate() { }

        public Coordinate(int x, int y)
        {
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
            if (serialized[0] != '{' || serialized[serialized.Length - 1] != '}')
                throw new ArgumentOutOfRangeException(nameof(serialized), "Must start with and end with {,}");
            var nums = serialized.Replace("{", String.Empty)
                .Replace("}", String.Empty)
                .Split(',');
            if (nums.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(serialized), "Must have two numbers separated by ,");

            if (int.TryParse(nums[0], out int x) && int.TryParse(nums[1], out int y))
            {
                return new Coordinate(x, y);
            }
            throw new ArgumentOutOfRangeException(nameof(serialized), "Unable to parse x and y");
        }

        [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
        public Coordinate LowerMiddle => new Coordinate(X, Y-1);
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Coordinate LowerRight => new Coordinate(X+1, Y-1);
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Coordinate LowerLeft => new Coordinate(X-1, Y-1);
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Coordinate Right => new Coordinate(X+1, Y);
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Coordinate Left => new Coordinate(X-1, Y);
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Coordinate UpperRight => new Coordinate(X+1, Y+1);
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Coordinate UpperMiddle => new Coordinate(X, Y+1);
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Coordinate UpperLeft => new Coordinate(X-1, Y+1);
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<Coordinate> Neighbors => new[] { LowerMiddle, LowerRight, LowerLeft, Right, Left, UpperRight, UpperMiddle, UpperLeft };

        public override bool Equals(object obj)
        {
            var other = obj as Coordinate;
            if (other == null)
                return false;
            return this == other;
        }

        public bool Equals(Coordinate other) => Equals((object)other);

        public static bool operator ==(Coordinate c1, Coordinate c2)
        {
            return (c1?.X == c2?.X && c1?.Y == c2?.Y);
        }

        public static bool operator !=(Coordinate c1, Coordinate c2) => !(c1 == c2);

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    }
}