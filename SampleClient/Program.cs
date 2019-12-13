using Contest.Shared;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleClient
{
    static class Program
    {
        static long generationsComputed = 0;
        static string token = null;
        static IEnumerable<Coordinate> solvedBoard = null;
        static IContestServer client = null;

        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Pass the server address as the 1st parameter.");
                return;
            }

            var server = args[0];
            Console.WriteLine($"Connecting to server @ {server}");

            client = RestService.For<IContestServer>(server);
            var registerRequest = new RegisterRequest { Name = Environment.UserName };
            var registerResponse = await client.Register(registerRequest);
            token = registerResponse.Token;

            UpdateResponse updateResponse = null;
            do
            {
                if (updateResponse != null)
                {
                    Console.WriteLine("Waiting for game to start... {0}", DateTime.Now);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                updateResponse = await client.Update(new UpdateRequest
                {
                    Token = token,
                    Status = ClientStatus.Waiting
                });
            } while (updateResponse.GameState == GameState.NotStarted);

            var seedBoard = updateResponse.SeedBoard;
            long generationsToCompute = updateResponse.GenerationsToCompute ?? 0;
            Console.WriteLine($"Got seed board w/{seedBoard.Count()} live cells counting {generationsToCompute} generations.");

            new Timer(new TimerCallback(heartbeat), state: null, dueTime: 1_200, period: 1_200);

            solvedBoard = GameSolver.Solve(seedBoard, generationsToCompute);

            Console.WriteLine("You finished!");
            Console.ReadLine();
        }
        private static void heartbeat(object state)
        {
            Console.WriteLine("\t[Reporting heartbeat]");
            var request = new UpdateRequest
            {
                Token = token,
                GenerationsComputed = generationsComputed,
                ResultBoard = solvedBoard,
                Status = ClientStatus.Complete
            };
            var response = client.Update(request);
        }
    }
}
