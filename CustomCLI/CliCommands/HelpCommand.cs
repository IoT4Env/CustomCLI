using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class HelpCommand : ICommand
{
    public static CommandSyntax? CheckSyntax(string[] args)
    {
        if (args.Length == 0)
        {
            return new CommandSyntax()
            {
                Arg = string.Empty
            };
        }
        Console.WriteLine("Arguments excedeed");
        return null;
    }

    /// <summary>
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">Default value is an empty string</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(CommandSyntax syntax) => true;

    /// <summary>
    /// print the command name followed by the description
    /// </summary>
    /// <param name="arg">Default value is an empty string</param>
    public static void Execute(CommandSyntax syntax)
    {
        Console.WriteLine("Command list:");
        foreach (CliCommandsClass command in CliCommandsClass.CliCommandsList)
            Console.Write($"{command.Name.ToString().ToLower()}\t{command.Description}\n");
    }
}
