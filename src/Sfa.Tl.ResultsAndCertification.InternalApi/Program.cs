using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.ResultsAndCertification.InternalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
                logging.AddFilter((category, level) => level >= (category == "Microsoft" ? LogLevel.Error : LogLevel.Information));
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
