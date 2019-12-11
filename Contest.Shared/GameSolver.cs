using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contest.Shared
{
    public class GameSolver
    {
        public static IEnumerable<Coordinate> Solve(IEnumerable<Coordinate> startingBoard, int numGenerations)
        {
            var resultBoard = new List<Coordinate>(startingBoard);

            for (int generation = numGenerations; generation > 0; generation--)
            {
                resultBoard = doGeneration(resultBoard);
            }

            return resultBoard;
        }

        private static List<Coordinate> doGeneration(List<Coordinate> board)
        {
            var result = new List<Coordinate>();
            foreach(var cell in board)
            {
                var cellNeighbors = FindNeighborCount(cell, board);

                if(cellNeighbors == 2 || cellNeighbors == 3)
                {
                    //cell lives.
                    result.Add(cell);
                }

                //neighbor cells might(?) become alive?
                foreach(var neighbor in cell.Neighbors)
                {
                    var neighborIsCurrentlyDead = !board.Any(c => c == neighbor);
                    if (neighborIsCurrentlyDead && FindNeighborCount(neighbor, board) == 3)
                        result.Add(neighbor);
                }
            }
            var distinct = result.Distinct();
            return distinct.ToList();
        }

        public static int FindNeighborCount(Coordinate cell, IEnumerable<Coordinate> board)
        {
            var neighbors = 0;
            if (board.Any(c => c == cell.UpperLeft))
                neighbors++;
            if (board.Any(c => c == cell.UpperMiddle))
                neighbors++;
            if (board.Any(c => c == cell.UpperRight))
                neighbors++;
            if (board.Any(c => c == cell.Left))
                neighbors++;
            if (board.Any(c => c == cell.Right))
                neighbors++;
            if (board.Any(c => c == cell.LowerLeft))
                neighbors++;
            if (board.Any(c => c == cell.LowerMiddle))
                neighbors++;
            if (board.Any(c => c == cell.LowerRight))
                neighbors++;
            return neighbors;
        }
    }
}
