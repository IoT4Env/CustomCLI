﻿using CustomCLI.CliCommands;
using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;


public class EchoCommand : ICommand
{
    /// <summary>
    /// Check if the syntax comply rules for this command
    /// </summary>
    /// <param name="args">Command arguments and/or options</param>
    /// <returns>A nullable CommandSyntax object</returns>
    public static CommandSyntax? CheckSyntax(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Argument required");
            return null;
        }

        var purifiedArgs = PurifyArgs(args);
        var syntax = EchoCommandOption.CheckSyntax(purifiedArgs);

        if (syntax is null)
        {
            return new CommandSyntax()
            {
                Arg = string.Join(" ", purifiedArgs[0..purifiedArgs.Length])
            };
        }
        return syntax;
    }

    /// <summary>
    /// Removes all empty strings inside the given string[]
    /// </summary>
    /// <param name="args">Argument to remove empty strings from</param>
    /// <returns>A new string[] without empty strings</returns>
    private static string[] PurifyArgs(string[] args) =>
        args.Where(arg => !string.IsNullOrEmpty(arg)).ToArray();

    /// <summary>
    /// Validates if the optionless command can execute
    /// </summary>
    /// <param name="syntax">Syntax object used for the command validation</param>
    /// <returns>Returns true if the command has no options</returns>
    public static bool CanExecute(CommandSyntax syntax) => syntax.Option is null;

    /// <summary>
    /// Prints the syntax argument in the terminal.
    /// </summary>
    /// <param name="syntax">Syntax object containing argument and/or options of the echo command</param>
    public static void Execute(CommandSyntax syntax) => Console.WriteLine(syntax.Arg);

    /// <summary>
    /// Prints the name of the given file system object in the terminal.
    /// </summary>
    /// <param name="fso">A generic file system object</param>
    public static void EchoFileSystemObject<FSO>(FSO fso) 
        where FSO : IFSO
    {
        Console.ForegroundColor = fso.Color;
        Console.Write(fso.Name.Contains(' ')
            ? $"\"{fso.Name}\" "
            : $"{fso.Name} ");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}

