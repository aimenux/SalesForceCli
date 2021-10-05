using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "SalesForceCli", FullName = "List/Query objects on SalesForce", Description = "List/Query objects on SalesForce.")]
    [Subcommand(typeof(ListCommand), typeof(QueryCommand))]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    public class MainCommand : AbstractCommand
    {
        public MainCommand(IConsoleHelper consoleHelper) : base(consoleHelper)
        {
        }

        [Option("-s|--settings", "Show settings information.", CommandOptionType.NoValue)]
        public bool ShowSettings { get; set; }

        protected override Task ExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            if (ShowSettings)
            {
                var filepath = GetSettingFilePath();
                ConsoleHelper.RenderSettingsFile(filepath);
            }
            else
            {
                const string title = "SalesForceCli";
                ConsoleHelper.RenderTitle(title);
                app.ShowHelp();
            }

            return Task.CompletedTask;
        }

        protected static string GetVersion() => GetVersion(typeof(MainCommand));

        private static string GetSettingFilePath() => Path.GetFullPath(Path.Combine(GetDirectoryPath(), @"appsettings.json"));

        private static string GetDirectoryPath()
        {
            try
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch
            {
                return Directory.GetCurrentDirectory();
            }
        }
    }
}