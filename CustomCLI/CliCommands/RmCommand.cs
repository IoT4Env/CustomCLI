﻿using CustomCLI.Commands.ICommands;
using CustomCLI.FileSystem;
using static CustomCLI.Kernel;
namespace CustomCLI.Commands;

public class RmCommand : ICommandComposite
{
    public static CommandSyntax? CheckSyntax(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Argument required");
        }
        else if (args.Length == 1)
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
    /// Tracks the specific file present somewhere in the file system and checks if the file exists
    /// </summary>
    /// <param name="compositePath">file name or path-to-file string</param>
    /// <returns>delete permission</returns>
    public static bool CanExecute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        VirtualFile file = GetFileByPosition(compositePath.LastArgName, offset);
        if (file is null)
        {
            Console.WriteLine($"No such file: {compositePath.LastArgName}");
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Removes the specified file
    /// </summary>
    /// <param name="compositePath">file name or path-to-file string</param>
    public static void Execute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        Dirs[offset].Files.RemoveAll(r => r.Name.Equals(compositePath.LastArgName));
    }
}
