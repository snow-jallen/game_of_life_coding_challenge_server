using Contest.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using FluentAssertions.Specialized;
using System.Text.Json;

namespace Contest.Tests
{
    [TestFixture]
    public class GameSolverTests
    {
        const string stillLifeII = "(1,1);(2,1);(2,-1);(2,-2);(2,-3);(1,-3);(4,1);(5,1);(4,-1);(4,-2);(4,-3);(5,-3)";
        const string diagonalS = "[{\"X\":1,\"Y\":1},{\"X\":1,\"Y\":2},{\"X\":2,\"Y\":1},{\"X\":2,\"Y\":3},{\"X\":3,\"Y\":3},{\"X\":4,\"Y\":3},{\"X\":4,\"Y\":5},{\"X\":5,\"Y\":4},{\"X\":5,\"Y\":5}]";
        const string leftLeaningO = "[{\"X\":-2,\"Y\":3},{\"X\":-2,\"Y\":4},{\"X\":-1,\"Y\":2},{\"X\":-1,\"Y\":5},{\"X\":1,\"Y\":1},{\"X\":1,\"Y\":4},{\"X\":2,\"Y\":2},{\"X\":2,\"Y\":3}]";
        const string gosperGliderGunInitial = "[{\"X\":12,\"Y\":-17},{\"X\":12,\"Y\":-16},{\"X\":13,\"Y\":-18},{\"X\":13,\"Y\":-14},{\"X\":14,\"Y\":-19},{\"X\":14,\"Y\":-13},{\"X\":14,\"Y\":-5},{\"X\":15,\"Y\":-29},{\"X\":15,\"Y\":-28},{\"X\":15,\"Y\":-19},{\"X\":15,\"Y\":-15},{\"X\":15,\"Y\":-13},{\"X\":15,\"Y\":-12},{\"X\":15,\"Y\":-7},{\"X\":15,\"Y\":-5},{\"X\":16,\"Y\":-29},{\"X\":16,\"Y\":-28},{\"X\":16,\"Y\":-19},{\"X\":16,\"Y\":-13},{\"X\":16,\"Y\":-9},{\"X\":16,\"Y\":-8},{\"X\":17,\"Y\":-18},{\"X\":17,\"Y\":-14},{\"X\":17,\"Y\":-9},{\"X\":17,\"Y\":-8},{\"X\":17,\"Y\":6},{\"X\":17,\"Y\":7},{\"X\":18,\"Y\":-17},{\"X\":18,\"Y\":-16},{\"X\":18,\"Y\":-9},{\"X\":18,\"Y\":-8},{\"X\":18,\"Y\":6},{\"X\":18,\"Y\":7},{\"X\":19,\"Y\":-7},{\"X\":19,\"Y\":-5},{\"X\":20,\"Y\":-5}]";
        const string gosperGliderGun43rdGen = "[{\"X\":6,\"Y\":-3},{\"X\":6,\"Y\":-2},{\"X\":6,\"Y\":-1},{\"X\":7,\"Y\":-1},{\"X\":8,\"Y\":-2},{\"X\":12,\"Y\":-21},{\"X\":12,\"Y\":-12},{\"X\":12,\"Y\":-9},{\"X\":13,\"Y\":-22},{\"X\":13,\"Y\":-20},{\"X\":13,\"Y\":-9},{\"X\":14,\"Y\":-24},{\"X\":14,\"Y\":-23},{\"X\":14,\"Y\":-19},{\"X\":14,\"Y\":-13},{\"X\":14,\"Y\":-12},{\"X\":14,\"Y\":-8},{\"X\":14,\"Y\":-5},{\"X\":15,\"Y\":-29},{\"X\":15,\"Y\":-28},{\"X\":15,\"Y\":-24},{\"X\":15,\"Y\":-23},{\"X\":15,\"Y\":-19},{\"X\":15,\"Y\":-14},{\"X\":15,\"Y\":-12},{\"X\":15,\"Y\":-10},{\"X\":15,\"Y\":-9},{\"X\":15,\"Y\":-5},{\"X\":15,\"Y\":-4},{\"X\":15,\"Y\":-3},{\"X\":15,\"Y\":-2},{\"X\":16,\"Y\":-29},{\"X\":16,\"Y\":-28},{\"X\":16,\"Y\":-24},{\"X\":16,\"Y\":-23},{\"X\":16,\"Y\":-19},{\"X\":16,\"Y\":-4},{\"X\":16,\"Y\":-3},{\"X\":16,\"Y\":-2},{\"X\":16,\"Y\":-1},{\"X\":17,\"Y\":-22},{\"X\":17,\"Y\":-20},{\"X\":17,\"Y\":-14},{\"X\":17,\"Y\":-13},{\"X\":17,\"Y\":-4},{\"X\":17,\"Y\":-1},{\"X\":17,\"Y\":6},{\"X\":17,\"Y\":7},{\"X\":18,\"Y\":-21},{\"X\":18,\"Y\":-4},{\"X\":18,\"Y\":-3},{\"X\":18,\"Y\":-2},{\"X\":18,\"Y\":-1},{\"X\":18,\"Y\":6},{\"X\":18,\"Y\":7},{\"X\":19,\"Y\":-5},{\"X\":19,\"Y\":-4},{\"X\":19,\"Y\":-3},{\"X\":19,\"Y\":-2},{\"X\":20,\"Y\":-5}]";

        [TestCase("(1,1)", "", 1, "Any live cell with fewer than two live neighbors dies as if by underpopulation (0 neighbors)")]
        [TestCase("(1,1);(1,2)", "", 1, "Any live cell with fewer than two live neighbors dies as if by underpopulation (1 neighbor)")]
        [TestCase("(1,1);(1,2);(2,1)", "(1,1);(1,2);(2,1);(2,2)", 1, "Any live cell with two or three live neighbors lives on to the next generation (2 neigbors)")]
        [TestCase("(1,1);(1,2);(2,1);(2,2)", "(1,1);(1,2);(2,1);(2,2)", 1, "Any live cell with two or three live neighbors lives on to the next generation (3 neigbors)")]
        [TestCase("(-1,2);(1,1);(1,2);(2,1);(2,2)", "(-1,2);(-1,1);(1,3);(2,1);(2,2)", 1, "Any live cell with more than three live neighbors dies, as if by overpopulation")]
        [TestCase("(1,1);(3,1);(2,3)","(2,2)", 1, "Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction")]
        [TestCase(stillLifeII, stillLifeII, 555,"stillLifeII doesn't ever change")]
        public void SolveBoard(string seed, string result, int numGenerations, string reason)
        {
            var seedBoard = seed.FromString();
            var expectedResult = result.FromString();

            var actualResult = GameSolver.Solve(seedBoard, numGenerations);

            expectedResult.Should().BeEquivalentTo(actualResult, because: reason);
        }

        [TestCase(diagonalS, diagonalS, 555, "Diagonal s doesn't move.  Ever.")]
        [TestCase(leftLeaningO, leftLeaningO, 555, "Left leaning O doesn't move.  Ever.")]
        [TestCase(gosperGliderGunInitial, gosperGliderGun43rdGen, 43, "Gun is shooting down and to the right")]
        public void SolveJsonBoard(string seed, string result, int numGenerations, string reason)
        {
            var seedBoard = JsonSerializer.Deserialize<IEnumerable<Coordinate>>(seed);
            var expectedResult = JsonSerializer.Deserialize<IEnumerable<Coordinate>>(result);

            var actualResult = GameSolver.Solve(seedBoard, numGenerations);

            expectedResult.Should().BeEquivalentTo(actualResult, because: reason);
        }

        [TestCase("(1,1);(1,2)", 1, Description = "1 neighbor")]
        [TestCase("(1,1);(1,2);(2,1)", 2, Description = "2 neighbors")]
        public void FindNeighbors(string seed, int expectedNeighbors)
        {
            var board = seed.FromString();
            int actualNeighbors = GameSolver.FindNeighborCount(board.First(), board);

            Assert.AreEqual(expectedNeighbors, actualNeighbors);
        }

        [Test]
        public void UpperLeftFrom1_1()
        {
            var starting = new Coordinate(1, 1);
            Assert.AreEqual(starting.UpperLeft, new Coordinate(-1, 2));
        }

        [Test]
        public void LowerRightFrom1_1()
        {
            var starting = new Coordinate(1, 1);
            Assert.AreEqual(starting.LowerRight, new Coordinate(2, -1));
        }
    }

    public static class SolverExtensions
    {
        public static IEnumerable<Coordinate> FromString(this string str)
        {
            var board = from cell in str.Split(";")
                        where cell.Contains("(")
                        let numbers = cell.Replace("(", "").Replace(")", "").Split(',')
                        let x = int.Parse(numbers[0])
                        let y = int.Parse(numbers[1])
                        select new Coordinate(x, y);
            return board;
        }

        public static IEnumerable<Coordinate> FromTable(this string table)
        {
            table = table.Replace("\t", "").Replace("\n","");
            var board = new List<Coordinate>();
            var rows = table.Split('\r',StringSplitOptions.RemoveEmptyEntries).ToList();
            for(int r=0; r < rows.Count; r++)
            {
                var row = rows[r];
                var cells = row.Split('|').ToList();
                for (int c = 0; c < cells.Count ; c++)
                {
                    if(cells[c].Contains("X"))
                        board.Add(new Coordinate(r, c));
                }
            }
            return board;
        }

        public static string ToTable(this IEnumerable<Coordinate> board)
        {
            var minX = board.Min(c => c.X);
            var maxX = board.Max(c => c.X);

            var minY = board.Min(c => c.Y);
            var maxY = board.Max(c => c.Y);

            var rows = new List<string>();
            for(int r=minX; r<=maxX; r++)
            {
                var cells = new List<string>();
                for(int c=minY;c<=maxY;c++)
                {
                    var cellValue = " ";
                    if (board.Any(b => b.X == r && b.Y == c))
                        cellValue = "X";
                    cells.Add(cellValue);
                }
                var row = $"|{String.Join('|', cells)}|";
                rows.Add(row);
            }
            var table = String.Join('\r', rows);
            return table;
        }
    }
}
