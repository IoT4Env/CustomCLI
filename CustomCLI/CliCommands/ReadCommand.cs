using CustomCLI.Commands.ICommands;

namespace CustomCLI.CliCommands;

public class ReadCommand : ICommand
{
    /// <summary>
    /// Verifies if this command can be executed
    /// </summary>
    /// <param name="syntax">The syntax of the argument</param>
    /// <returns>true</returns>
    public static bool CanExecute(CommandSyntax syntax)
    {
        string variable = Console.ReadLine() ?? string.Empty;
        if (variable == null)
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

        Console.WriteLine("Arguments exceede");
        return null;
    }

    /// <summary>
    /// Stores the standard input content in a variable
    /// </summary>
    /// <param name="syntax">Syntax object containing the variable name as the argument</param>
    public static void Execute(CommandSyntax syntax)
    {
        string variable = Console.ReadLine() ?? string.Empty;

        Console.WriteLine(variable);
    }
}
