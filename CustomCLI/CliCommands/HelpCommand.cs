using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class HelpCommand : ICommand
{
    public static void CheckSyntax(CommandSyntax syntax)
    {
        //return true;
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
        foreach (CliCommandsEnum command in Enum.GetValues(typeof(CliCommandsEnum)))
            Console.Write($"{command.ToString().ToLower()}\t{command.ToDescriptionString()}\n");
    }
}
