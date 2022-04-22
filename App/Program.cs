using System.Threading.Tasks;
using App.Commands;
using App.Extensions;
using Lib.Configuration;
using Lib.Helpers;
using Lib.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            return CreateHostBuilder(args).RunCommandLineApplicationAsync<MainCommand>(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddJsonFile();
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConsoleLogger();
                    loggingBuilder.AddNonGenericLogger();
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddTransient<MainCommand>();
                    services.AddTransient<ListCommand>();
                    services.AddTransient<QueryCommand>();
                    services.AddTransient<IFileHelper, FileHelper>();
                    services.AddTransient<IConsoleHelper, ConsoleHelper>();
                    services.AddTransient<ISalesForceService, SalesForceService>();
                    services.Configure<Settings>(hostingContext.Configuration.GetSection(nameof(Settings)));
                });
    }
}
