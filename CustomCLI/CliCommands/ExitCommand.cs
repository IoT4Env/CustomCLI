using CustomCLI.Commands.ICommands;
using System.Collections.Generic;

namespace CustomCLI.CliCommands;

public class ExitCommand : ICommand
{
    public static bool CanExecute(CommandSyntax syntax) => true;

    public static CommandSyntax? CheckSyntax(string[] syntax)
    {
        return null;
    }

    public static void Execute(CommandSyntax syntax)
    {
        Kernel.IsExit = true;
    }
}
