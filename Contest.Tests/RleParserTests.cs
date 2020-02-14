using Contest.Shared;
using Contest.Shared.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contest.Tests
{
    [TestFixture]
    public class RleParserTests
    {
        [Test]
        public void CanParseBasicRle()
        {
            var rle = @"
x = 3, y = 3, rule = B3/S23
2bo$o$2o!
";
            var actualBoard = RleParser.FromRle(rle);
            var expectedBoard = new[] { new Coordinate(3, 3), new Coordinate(1, 2), new Coordinate(1, 1), new Coordinate(2, 1) };

            actualBoard.Should().BeEquivalentTo(expectedBoard);
        }

        [Test]
        public void CanHandleSingleBoardLineSpreadOverMultipleTextFileLines()
        {
            var rle = @"
x = 3, y = 3, rule = B3/S23
2b
o$o$2o!
";
            var actualBoard = RleParser.FromRle(rle);
            var expectedBoard = new[] { new Coordinate(3, 3), new Coordinate(1, 2), new Coordinate(1, 1), new Coordinate(2, 1) };

            actualBoard.Should().BeEquivalentTo(expectedBoard);
        }

        [Test]
        public void CanHandleBlankRows()
        {
            var rle = @"
x = 8, y = 10, rule = B3/S23
2bo$o$2o3$3bo2$5bo2$7bo!
";
            var actualBoard = RleParser.FromRle(rle);
            var expectedBoard = new[] {
                new Coordinate(3, 10),
                new Coordinate(1, 9),
                new Coordinate(1, 8), new Coordinate(2, 8),


                new Coordinate(4,5),

                new Coordinate(6, 3),

                new Coordinate(8, 1)
            };
            actualBoard.Should().BeEquivalentTo(expectedBoard);
        }

        [Test]
        public void CanWriteOutRle()
        {
            var board = new[] {
                new Coordinate(3, 10),
                new Coordinate(1, 9),
                new Coordinate(1, 8), new Coordinate(2, 8),


                new Coordinate(4,5),

                new Coordinate(6, 3),

                new Coordinate(8, 1)
            };

            var actualRle = RleParser.ToRle(board).Trim();
            var expectedRle = @"
x = 8, y = 10, rule = B3/S23
2bo$o$2o3$3bo2$5bo2$7bo!
".Trim();
            actualRle.Should().Be(expectedRle);
        }
    }
}
