﻿using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;
using System.Text.RegularExpressions;

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
        var syntax = EchoCommandOptions.CheckSyntax(purifiedArgs);

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
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">string to be displayed on the terminal</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(CommandSyntax syntax) => true;

    /// <summary>
    /// Print the specified string on the terminal.
    /// </summary>
    /// <param name="arg">string to be dislayed on the terminal</param>
    public static void Execute(CommandSyntax syntax)
    {
        if (syntax.Option is null)
        {
            Echo(syntax.Arg);
            return;
        }

        if (EchoCommandOptions.CanExecute(syntax))
            EchoCommandOptions.Execute(syntax);
    }

    private static void Echo(string arg) => Console.WriteLine(arg);

    public static void EchoFileName(VirtualFile file)
    {
        Console.ForegroundColor = file.Color;
        Console.Write(file.Name.Contains(' ')
            ? $"\"{file.Name}\" "
            : $"{file.Name} ");
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void EchoFolderName(VirtualFolder folder)
    {
        Console.ForegroundColor = folder.Color;
        Console.Write(folder.Name.Contains(' ')
            ? $"\"{folder.Name}\" "
            : $"{folder.Name} ");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}

class EchoCommandOptions : ICommand
{
    /// <summary>
    /// A string whose chars are associated with options of the echo command
    /// </summary>
    private static readonly string Options = "neE";

    public static bool CanExecute(CommandSyntax syntax)
    {
        if (syntax.Option is null)
        {
            Console.WriteLine($"Argument cannot be null");
            return false;
        }

        if (syntax.Option.Length > Options.Length)
        {
            Console.WriteLine("Too many arguments");
            return false;
        }

        foreach (char option in syntax.Option)
        {
            if (!Options.Contains(option))
            {
                Console.WriteLine($"Invalid command option: {option}");
                return false;
            }
        }

        if (syntax.Option.Contains('e') && syntax.Option.Contains('E'))
        {
            Console.WriteLine($"Arguments 'e' and 'E' cannot be specified together");
            return false;
        }

        return true;
    }

    public static CommandSyntax? CheckSyntax(string[] args)
        => args[0].Contains('-') && args.Length > 1
            ? new CommandSyntax
            {
                Arg = string.Join(" ", args[1..args.Length]),
                Option = args[0].Replace("-", "")
            }
            : null;

    public static void Execute(CommandSyntax syntax)
    {
        var output = syntax.Arg;
        if (syntax.Option.Contains('e'))
        {
            output = Regex.Unescape(syntax.Arg);
        }
        if (syntax.Option.Contains('n'))
        {
            Console.Write(output);
            return;
        }
        Console.WriteLine(output);
    }
}
