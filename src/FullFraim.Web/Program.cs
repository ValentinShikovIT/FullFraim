using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace FullFraim.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Logging for application.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(path: "Logs\\LoggedInformation.txt",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                    fileSizeLimitBytes: 1024 * 1024, // 1Mb
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
