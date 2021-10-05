using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Lib.Helpers;
using Lib.Services;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Generate", FullName = "Generate SalesForce Query", Description = "Generate SalesForce Query.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class GenerateCommand : AbstractCommand
    {
        private readonly ISalesForceService _salesForceService;

        public GenerateCommand(ISalesForceService salesForceService, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _salesForceService = salesForceService;
        }

        [Range(1, 1000)]
        [Option("-m|--max", "MaxItems", CommandOptionType.SingleValue)]
        public int MaxItems { get; set; } = 30;

        [Required]
        [Option("-n|--name", "ObjectName", CommandOptionType.SingleValue)]
        public string ObjectName { get; set; }

        protected override async Task ExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            var parameters = new SalesForceParameters
            {
                MaxItems = MaxItems,
                ObjectName = ObjectName
            };

            var query = await _salesForceService.GetQueryAsync(parameters, cancellationToken);
            ConsoleHelper.RenderQuery(ObjectName, query);
        }

        protected override bool HasValidOptions()
        {
            return !string.IsNullOrWhiteSpace(ObjectName);
        }

        protected static string GetVersion() => GetVersion(typeof(GenerateCommand));
    }
}