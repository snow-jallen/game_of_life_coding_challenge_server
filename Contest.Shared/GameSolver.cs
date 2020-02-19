using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contest.Shared.Models;

namespace Contest.Shared
{
    public class GameSolver
    {
        public static IEnumerable<Coordinate> Solve(IEnumerable<Coordinate> startingBoard, long numGenerations, int batchSize=50)
        {
            var resultBoard = new List<Coordinate>(startingBoard);

            for(long generation = 0; generation < numGenerations; generation++)
            {
                Console.Write($"{generation} ");
                resultBoard = doGeneration(resultBoard);

                if (generation % batchSize == 0)
                    GenerationBatchCompleted?.Invoke(null, new SolverEventArgs(generation));
            }

            return resultBoard;
        }

        public static event EventHandler<SolverEventArgs> GenerationBatchCompleted;

        private static List<Coordinate> doGeneration(List<Coordinate> board)
        {
            var result = new ConcurrentBag<Coordinate>();
            Parallel.ForEach(board, (cell) =>
            {
                var cellNeighbors = FindNeighborCount(cell, board);

                if (cellNeighbors == 2 || cellNeighbors == 3)
                {
                    //cell lives.
                    result.Add(cell);
                }

                //neighbor cells might(?) become alive?
                foreach (var neighbor in cell.Neighbors)
                {
                    var neighborIsCurrentlyDead = !board.Any(c => c == neighbor);
                    if (neighborIsCurrentlyDead && FindNeighborCount(neighbor, board) == 3)
                        result.Add(neighbor);
                }
            });
            var distinct = result.Distinct();
            return distinct.ToList();
        }

        public static int FindNeighborCount(Coordinate cell, IEnumerable<Coordinate> board) =>
            board.Count(c => c?.IsNeighbor(cell) ?? false);
    }

    public class SolverEventArgs : EventArgs
    {
        public SolverEventArgs(long generationsComputed)
        {
            GenerationsComputed = generationsComputed;
        }

        public long GenerationsComputed { get; set; }
    }
}
