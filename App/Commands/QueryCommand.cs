using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Attributes;
using Lib.Helpers;
using Lib.Services;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Query", FullName = "Query SalesForce", Description = "Query SalesForce.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class QueryCommand : AbstractCommand
    {
        private readonly ISalesForceService _salesForceService;

        public QueryCommand(ISalesForceService salesForceService, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _salesForceService = salesForceService;
        }

        [Required]
        [FileValidator]
        [Option("-f|--file", "Query File", CommandOptionType.SingleValue)]
        public string FileName { get; set; }

        protected override async Task ExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            var parameters = new SalesForceParameters
            {
                FileName = FileName
            };

            var results = await _salesForceService.RunQueryAsync<ExpandoObject>(parameters, cancellationToken);
            ConsoleHelper.RenderSalesForceResults(results);
        }

        protected override bool HasValidOptions()
        {
            return !string.IsNullOrWhiteSpace(FileName) && File.Exists(FileName);
        }

        protected static string GetVersion() => GetVersion(typeof(QueryCommand));
    }
}
