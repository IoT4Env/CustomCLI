﻿using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class EchoCommand : ICommand
{
    /// <summary>
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">string to be displayed on the terminal</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(string arg) => true;

    /// <summary>
    /// print the specified string on the terminal
    /// </summary>
    /// <param name="arg">string to be dislayed on the terminal</param>
    public static void Execute(string arg) => Console.WriteLine(arg);
}
