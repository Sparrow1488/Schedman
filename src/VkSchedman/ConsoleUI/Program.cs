using Spectre.Console;
using System;

namespace ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.Record();
            AnsiConsole.Write(new FigletText("Sparrow1488")
                              .LeftAligned()
                              .Color(Color.Red));
            AnsiConsole.MarkupLine("[green]This is all green[/] [red]And this is all red![/]");
            string fileName = "megaLogFile.txt";
            bool saveFile = AnsiConsole.Confirm($"Save [yellow]{fileName}[/] file?");
            AnsiConsole.MarkupLine(saveFile.ToString());
            var framework = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                               .Title("Select [green]test framework[/] to use")
                                               .PageSize(10)
                                               .MoreChoicesText("[grey](Move up and down to reveal more frameworks)[/]")
                                               .AddChoices(new[] {
                                                   "XUnit", "NUnit","MSTest"
                                               }));
            var html = AnsiConsole.ExportHtml();
        }
    }
}
