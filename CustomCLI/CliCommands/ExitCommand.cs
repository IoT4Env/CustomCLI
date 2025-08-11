using CustomCLI.Commands.ICommands;
using System.Collections.Generic;

namespace CustomCLI.CliCommands;

public class ExitCommand : ICommand
{
    public static bool CanExecute(CommandSyntax syntax) => true;

    public static CommandSyntax? CheckSyntax(string[] args)
    {
        //If no arguments where given
        if (args.Length == 1)
        {
            return new CommandSyntax()
            {
                Arg = args[0]
            };
        }
        Console.WriteLine("Arguments excedeed");
        return null;
    }

    public static void Execute(CommandSyntax syntax)
    {
        Kernel.IsExit = true;
    }
}
