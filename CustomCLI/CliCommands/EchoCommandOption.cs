using CustomCLI.Commands.ICommands;
using System.Text.RegularExpressions;
using static CustomCLI.Memory;

namespace CustomCLI.CliCommands;

public class EchoCommandOption : ICommand
{
    /// <summary>
    /// A string whose chars are associated with options of the echo command
    /// </summary>
    private static readonly string Options = "neE";

    /// <summary>
    /// Verify that a series of contrains are met
    /// </summary>
    /// <param name="syntax">CommandSyntax object</param>
    /// <returns>True when all validation have passed</returns>
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

    /// <summary>
    /// Checks the right syntax for the given option(s)
    /// </summary>
    /// <param name="syntax">CommandSyntax object</param>
    /// <returns>A nullable CommandSyntax object</returns>
    public static CommandSyntax? CheckSyntax(string[] args)
    {
        var newArgs = ResolveVariables(args);
        return newArgs[0].Contains('-') && newArgs.Length > 1
            ? new CommandSyntax
            {
                Arg = string.Join(" ", newArgs[1..newArgs.Length]),
                Option = newArgs[0].Replace("-", "")
            }
            : null;
    }

    /// <summary>
    /// Executes the command with the associated arguments
    /// </summary>
    /// <param name="syntax">CommandSyntax object</param>
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
