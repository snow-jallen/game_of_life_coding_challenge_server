using Contest.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contest.Tests
{
    [TestFixture]
    public class GameSolverTests
    {		
		[TestCase("(1,1)", "", 1, Description = "Any live cell with fewer than two live neighbors dies as if by underpopulation")]
		public void SolveBoard(string seed, string result, int numGenerations)
		{
			var seedBoard = seed.FromString();
			var expectedResult = result.FromString();

			var actualResult = GameSolver.Solve(seedBoard, numGenerations);

			Assert.AreEqual(expectedResult, actualResult);
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
