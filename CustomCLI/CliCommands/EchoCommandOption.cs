using CustomCLI.Commands.ICommands;
using System.Text.RegularExpressions;

namespace CustomCLI.CliCommands;

public class EchoCommandOption : ICommand
{
    /// <summary>
    /// A string whose chars are associated with options of the echo command
    /// </summary>
    private static readonly string Options = "neE";

    public static bool CanExecute(CommandSyntax syntax)
    {
        if (syntax.Option is null)
        {
            Console.WriteLine($"Options cannot be null");
            return false;
        }

        if (syntax.Option.Length > Options.Length)
        {
            Console.WriteLine("Too many options");
            return false;
        }

        foreach (char option in syntax.Option)
        {
            if (!Options.Contains(option))
            {
                Console.WriteLine($"Invalid command option: {option}");
                return false;
            }
        }

        if (syntax.Option.Contains('e') && syntax.Option.Contains('E'))
        {
            Console.WriteLine($"Options 'e' and 'E' cannot be specified together");
            return false;
        }

        return true;
    }

    public static CommandSyntax? CheckSyntax(string[] args)
        => args[0].Contains('-') && args.Length > 1
            ? new CommandSyntax
            {
                Arg = string.Join(" ", args[1..args.Length]),
                Option = args[0].Replace("-", "")
            }
            : null;

    public static void Execute(CommandSyntax syntax)
    {
        var output = syntax.Arg;
        if (syntax.Option.Contains('e'))
        {
            output = Regex.Unescape(syntax.Arg);
        }
        if (syntax.Option.Contains('n'))
        {
            Console.Write(output);
            return;
        }
        Console.WriteLine(output);
    }
}
