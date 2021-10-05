using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using App.Attributes;
using Lib.Helpers;
using Lib.Services;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "List", FullName = "List SalesForce objects/fields", Description = "List SalesForce objects/fields.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class ListCommand : AbstractCommand
    {
        private readonly ISalesForceService _salesForceService;

        public ListCommand(ISalesForceService salesForceService, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _salesForceService = salesForceService;
        }

        [Range(1, 1000)]
        [Option("-m|--max", "MaxItems", CommandOptionType.SingleValue)]
        public int MaxItems { get; set; } = 30;

        [PatternValidator]
        [Option("-p|--pattern", "Pattern matching", CommandOptionType.SingleValue)]
        public string Pattern { get; set; }

        [Option("-n|--name", "ObjectName", CommandOptionType.SingleValue)]
        public string ObjectName { get; set; }

        protected override async Task ExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            var parameters = new SalesForceParameters
            {
                Pattern = Pattern,
                MaxItems = MaxItems,
                ObjectName = ObjectName
            };

            var caption = string.IsNullOrWhiteSpace(Pattern) ? null : $"Pattern is {Pattern}";

            if (string.IsNullOrWhiteSpace(ObjectName))
            {
                var objects = await _salesForceService.GetObjectsAsync(parameters, cancellationToken);
                ConsoleHelper.RenderSalesForceObjects(objects, caption);
            }
            else
            {
                var fields = await _salesForceService.GetFieldsAsync(parameters, cancellationToken);
                ConsoleHelper.RenderSalesForceFields(fields, caption);
            }
        }

        protected static string GetVersion() => GetVersion(typeof(ListCommand));
    }
}