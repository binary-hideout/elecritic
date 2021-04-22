using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace Elecritic {
    public class Program {

        public static void Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try {
                Log.Information("Starting web host.");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception) {
                Log.Fatal(exception, "Host terminated unexpectedly.");
            }
            finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
