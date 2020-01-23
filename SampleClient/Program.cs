using Contest.Shared;
using Contest.Shared.Enums;
using Contest.Shared.Http;
using Contest.Shared.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleClient
{
    static class Program
    {
        static long generationsComputed = 0;
        static long generationsToCompute = 0;
        static string token = null;
        static IEnumerable<Coordinate> solvedBoard = null;
        static IContestServer client = null;
        static ClientStatus status;
        static Timer heartbeatTimer;

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
            status = ClientStatus.Waiting;

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
                    Status = status
                });
            } while (updateResponse.GameState == GameState.NotStarted);

            var seedBoard = updateResponse.SeedBoard;
            generationsToCompute = updateResponse.GenerationsToCompute ?? 0;
            Console.WriteLine($"Got seed board w/{seedBoard.Count()} live cells counting {generationsToCompute} generations.");

            heartbeatTimer = new Timer(new TimerCallback(heartbeat), state: null, dueTime: 500, period: 500);

            status = ClientStatus.Processing;
            GameSolver.GenerationBatchCompleted += (s, e) => Interlocked.Exchange(ref generationsComputed, e.GenerationsComputed);
            solvedBoard = GameSolver.Solve(seedBoard, generationsToCompute, batchSize: 5);
            status = ClientStatus.Complete;


            Console.WriteLine("You finished!");
            Console.ReadLine();
        }

        private static async void heartbeat(object state)
        {
            var generations = (status == ClientStatus.Complete) ?
                generationsToCompute :
                Interlocked.Read(ref generationsComputed);

            var request = new UpdateRequest
            {
                Token = token,
                GenerationsComputed = generations,
                ResultBoard = solvedBoard,
                Status = status
            };
            Console.WriteLine($"\t[Reporting heartbeat: Status={status}; Generations={generations}]");
            var response = await client.Update(request);

            if(status == ClientStatus.Complete)
            {
                if(response.GameState != GameState.Over)
                    Console.WriteLine("Waiting for server to say game is over.");
                else
                {
                    Console.WriteLine("All done, good game!");
                    heartbeatTimer.Dispose();//stop the timer.
                }
            }
        }
    }
}
