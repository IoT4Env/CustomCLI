using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class ClsCommand : ICommand
{
    public static CommandSyntax? CheckSyntax(string[] args)
    {
        //If no arguments where given
        if(args.Length == 1)
        {
            return new CommandSyntax()
            {
                Arg = args[0]
            };
        }
        Console.WriteLine("Arguments excedeed");
        return null;
    }
    /// <summary>
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">string to be displayed on the terminal</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(CommandSyntax syntax) => true;

    /// <summary>
    /// clears the terminal output
    /// </summary>
    /// <param name="arg">string to be displayed on the terminal</param>
    public static void Execute(CommandSyntax syntax) => Console.Clear();
}
