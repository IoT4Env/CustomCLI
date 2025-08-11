using CustomCLI.Commands.ICommands;
using static CustomCLI.Kernel;

namespace CustomCLI.CliCommands;

public class ReadCommand : ICommand
{
    private static string Content { get; set; } = string.Empty;
    /// <summary>
    /// Verifies if this command can be executed
    /// </summary>
    /// <param name="syntax">The syntax of the argument</param>
    /// <returns>true</returns>
    public static bool CanExecute(CommandSyntax syntax)
    {
        Content = Console.ReadLine() ?? string.Empty;
        if (Content == null)
        {
            Console.WriteLine("Cannot read null");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Verifies that the variable name that stores the inputted value is provided
    /// </summary>
    /// <param name="args">Command arguments</param>
    /// <returns>A CommandSyntax object</returns>
    public static CommandSyntax? CheckSyntax(string[] args)
    {
        if(args.Length == 0)
        {
            Console.WriteLine("Argument required");
            return null;
        }

        if (args.Length == 1)
        {
            return new CommandSyntax()
            {
                Arg = args[0]
            };
        }

        Console.WriteLine("Arguments exceeded");
        return null;
    }

    /// <summary>
    /// Stores the standard input content in a variable
    /// It allocates memory in the Heap for the content stored inside the variable
    /// If the variable name is already used, then its content is replaced with the new value
    /// </summary>
    /// <param name="syntax">Syntax object containing the variable name as the argument</param>
    public static void Execute(CommandSyntax syntax)
    {
        if (Heap.ContainsKey($"${syntax.Arg}"))
        {
            Heap[$"${syntax.Arg}"] = Content;
            return;
        }
        Heap.Add($"${syntax.Arg}", Content);
    }
}
