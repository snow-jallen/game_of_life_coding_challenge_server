using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Loki;

namespace ContestServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UsePort();
                })
            .UseSerilog((context, loggerConfig) => {
                loggerConfig.WriteTo.Console()
                .Enrich.WithExceptionDetails()
                .WriteTo.LokiHttp(()=> new LokiSinkConfiguration
                {
                    LokiUrl = "http://loki:3100"
                }) ;
            });
    }


    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UsePort(this IWebHostBuilder builder)
        {
            var port = Environment.GetEnvironmentVariable("PORT");
            if (string.IsNullOrEmpty(port))
                return builder;
            return builder.UseUrls($"http://+:{port}");
        }
    }
}
