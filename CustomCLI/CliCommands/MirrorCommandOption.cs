using CustomCLI.Commands;
using CustomCLI.Commands.ICommands;
using static CustomCLI.Memory;

namespace CustomCLI.CliCommands;

public class MirrorCommandOption : ICommand
{
    /// <summary>
    /// A string whose chars are associated with options of the mirror command
    /// </summary>
    private static readonly string Options = "f";
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

        if(syntax.Option.Contains('f') && string.IsNullOrEmpty(Path.GetFileName(syntax.Arg)))
        {
            Console.WriteLine($"No such file: {syntax.Arg}");
            return false;
        }

        return true;
    }

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

    public static void Execute(CommandSyntax syntax)
    {
        if (syntax.Option.Contains('f'))
        {
            MirrorCommand.GetRealFiles(syntax.Arg);
        }
    }
}
