using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    public abstract class AbstractCommand
    {
        protected IConsoleHelper ConsoleHelper;

        protected AbstractCommand(IConsoleHelper consoleHelper)
        {
            ConsoleHelper = consoleHelper;
        }

        public async Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!HasValidOptions() || !HasValidArguments())
                {
                    throw new Exception($"Invalid options/arguments for command {GetType().Name}");
                }

                await ExecuteAsync(app, cancellationToken);
            }
            catch (Exception ex)
            {
                ConsoleHelper.RenderException(ex);
            }

            return 0;
        }

        protected abstract Task ExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default);

        protected virtual bool HasValidOptions() => true;

        protected virtual bool HasValidArguments() => true;

        protected static string GetVersion(Type type)
        {
            return type
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
        }
    }
}
