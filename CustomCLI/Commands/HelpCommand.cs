using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class HelpCommand : ICommand
{
    /// <summary>
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">Default value is an empty string</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(string arg = "") => true;

    /// <summary>
    /// print the command name followed by the description
    /// </summary>
    /// <param name="arg">Default value is an empty string</param>
    public static void Execute(string arg = "")
    {
        Console.WriteLine("Command list:");
        foreach (CliCommands command in Enum.GetValues(typeof(CliCommands)))
            Console.Write($"{command.ToString().ToLower()}\t{command.ToDescriptionString()}\n");
    }
}
