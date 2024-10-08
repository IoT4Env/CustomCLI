﻿using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;
using CustomCLI.CliCommands.Resources;

namespace CustomCLI.Commands;

public class LsCommand : ICommand
{
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

    /// <summary>
    /// Validates if the command can execute
    /// </summary>
    /// <param name="arg">setted by default as an empty string</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(CommandSyntax syntax) => true;

    /// <summary>
    /// Prints all files and folders inside the current directory
    /// </summary>
    /// <param name="arg">setted by default as an empty string</param>
    public static void Execute(CommandSyntax syntax)
    {
        VirtualFolder? dir = GetCurrentDir();
        BrowseDirectory(dir, EchoCommand.EchoFileName, EchoCommand.EchoFolderName);
    }
}
