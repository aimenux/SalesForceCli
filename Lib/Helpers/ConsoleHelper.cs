using System;
using System.Collections.Generic;
using System.IO;
using Lib.Extensions;
using Lib.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;

namespace Lib.Helpers
{
    public class ConsoleHelper : IConsoleHelper
    {
        public void RenderTitle(string text)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new FigletText(text).LeftAligned());
            AnsiConsole.WriteLine();
        }

        public void RenderSettingsFile(string filepath)
        {
            var name = Path.GetFileName(filepath);
            var json = File.ReadAllText(filepath);
            var formattedJson = JToken.Parse(json).ToString(Formatting.Indented);
            var header = new Rule($"[yellow]({name})[/]");
            header.Centered();
            var footer = new Rule($"[yellow]({filepath})[/]");
            footer.Centered();

            AnsiConsole.WriteLine();
            AnsiConsole.Render(header);
            AnsiConsole.WriteLine(formattedJson);
            AnsiConsole.Render(footer);
            AnsiConsole.WriteLine();
        }

        public void RenderSalesForceFields(ICollection<string> fields, string caption = null)
        {
            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title($"[yellow]{fields.Count} field(s)[/]")
                .AddColumn(new TableColumn("[u]Name[/]").Centered());

            if (!string.IsNullOrWhiteSpace(caption))
            {
                table.Caption($"[grey]{caption}[/]");
            }

            foreach (var field in fields)
            {
                table.AddRow(field.GetValueOrEmpty());
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderSalesForceResults<T>(SalesForceResults<T> results, string caption = null)
        {
            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title($"[yellow]{results.Records.Count} record(s)[/]")
                .AddColumn(new TableColumn("[u]Input[/]").Centered())
                .AddColumn(new TableColumn("[u]Output[/]").Centered());

            if (!string.IsNullOrWhiteSpace(caption))
            {
                table.Caption($"[grey]{caption}[/]");
            }

            var inputFileMarkup = $"[green][link={results.InputFile}]{results.InputFile}[/][/]";
            var outputFileMarkup = $"[green][link={results.OutputFile}]{results.OutputFile}[/][/]";

            table.AddRow(inputFileMarkup, outputFileMarkup);

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderSalesForceObjects(ICollection<SalesForceObject> objects, string caption = null)
        {
            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title($"[yellow]{objects.Count} object(s)[/]")
                .AddColumn(new TableColumn("[u]Name[/]").Centered())
                .AddColumn(new TableColumn("[u]Label[/]").Centered());

            if (!string.IsNullOrWhiteSpace(caption))
            {
                table.Caption($"[grey]{caption}[/]");
            }

            foreach (var obj in objects)
            {
                table.AddRow(obj.Name.GetValueOrEmpty(), obj.Label.GetValueOrEmpty());
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderException(Exception exception)
        {
            const ExceptionFormats formats = ExceptionFormats.ShortenTypes
                                             | ExceptionFormats.ShortenPaths
                                             | ExceptionFormats.ShortenMethods;

            AnsiConsole.WriteLine();
            AnsiConsole.WriteException(exception, formats);
            AnsiConsole.WriteLine();
        }
    }
}