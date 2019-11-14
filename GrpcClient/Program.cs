using Grpc.Net.Client;
using GrpcServer;
using System;

namespace GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press [enter] when you're ready to connect");
            Console.ReadLine();

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(new HelloRequest { Name = "Jonathan" });
            Console.WriteLine($"Greeting: {reply.Message}");
            Console.ReadKey();
        }
    }
}
