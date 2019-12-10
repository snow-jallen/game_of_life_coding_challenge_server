using Contest.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using FluentAssertions.Specialized;

namespace Contest.Tests
{
    [TestFixture]
    public class GameSolverTests
    {		
		[TestCase("(1,1)", "", 1, "Any live cell with fewer than two live neighbors dies as if by underpopulation (0 neighbors)")]
		[TestCase("(1,1);(1,2)", "", 1, "Any live cell with fewer than two live neighbors dies as if by underpopulation (1 neighbor)")]
		[TestCase("(1,1);(1,2);(2,1)", "(1,1);(1,2);(2,1);(2,2)", 1, "Any live cell with two or three live neighbors lives on to the next generation (2 neigbors)")]
		[TestCase("(1,1);(1,2);(2,1);(2,2)", "(1,1);(1,2);(2,1);(2,2)", 1, "Any live cell with two or three live neighbors lives on to the next generation (3 neigbors)")]
		[TestCase("(-1,2);(1,1);(1,2);(2,1);(2,2)", "(-1,2);(-1,1);(1,3);(2,1);(2,2)", 1, "Any live cell with more than three live neighbors dies, as if by overpopulation")]
		[TestCase("(1,1);(3,1);(2,3)","(2,2)", 1, "Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction")]
		public void SolveBoard(string seed, string result, int numGenerations, string reason)
		{
			var seedBoard = seed.FromString();
			var expectedResult = result.FromString();

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
