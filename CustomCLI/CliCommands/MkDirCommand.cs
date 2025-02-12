﻿using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;
using CustomCLI.FileSystem;

namespace CustomCLI.Commands;

public class MkDirCommand : ICommandComposite
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
    /// Tracks the specific directory present somewhere in the file system and checks if it already exists
    /// </summary>
    /// <param name="compositePath">directory name</param>
    /// <returns>create permission</returns>
    public static bool CanExecute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;//subtract 2 because of 2 lengths (not indexes) added togeter
        VirtualFolder dir = GetDirectoryByPosition(compositePath.LastArgName, offset);

        if (dir is not null)
        {
            Console.WriteLine($"Directory {compositePath.LastArgName} already exists");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Creates a folder inside the current folder AND adds the folder in the Dirs list.
    /// </summary>
    /// <param name="compositePath">directory name (not working with paths like folder1/folder2 yet)</param>
    public static void Execute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;

        //horizontal scaling:
        //Inside one folder there are n folders (and files)
        var foldersCheck = compositePath.Folders.Equals(string.Empty) ? "" : $"{compositePath.Folders}/";

        //join the position of the command execution, the folders provided to the argument, and the folder name
        var path = $"{string.Join('/', Tree)}/{foldersCheck}{compositePath.LastArgName}";
        Dirs[offset].Folders.Add(new VirtualFolder
        {
            Color = ConsoleColor.Blue,
            Name = compositePath.LastArgName,
            Path = path
        });

        //a reduncancy of the previus VirtualFolder
        //lets the ls command know in which folder the user currently is
        Dirs.Add(new VirtualFolder
        {
            Dept = offset,
            Name = compositePath.LastArgName,
            Path = path
        });
    }
}
