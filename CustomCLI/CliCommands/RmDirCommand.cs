using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;
using CustomCLI.CliCommands.Resources;
using System.Diagnostics;

namespace CustomCLI.Commands;

public class RmDirCommand : ICommandComposite
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
    /// Checks if the folder exists and if it is empty
    /// </summary>
    /// <param name="compositePath">directory name (not working with paths like folder1/folder2 yet)</param>
    /// <returns>delete permission</returns>
    public static bool CanExecute(CompositePath compositePath)
    {
        if (!FolderExists(compositePath.LastArgName))
        {
            Console.WriteLine($"No such folder: {compositePath.LastArgName}");
            return false;
        }

        var offset = Tree.Count + compositePath.ArgsNum - 2;
        VirtualFolder targetFolder = GetDirectoryByPosition(compositePath.LastArgName, offset);
        if (!IsFolderEmpty(targetFolder))
        {
            Console.WriteLine($"\n{compositePath.LastArgName} contains the following elements:");

            BrowseDirectory(targetFolder, EchoCommand.EchoFileSystemObject);

            Console.WriteLine("Would you like to remove them? [y/N]");
            string? response = Console.ReadLine();
            if (response.Equals("y"))
            {
                BrowseDirectory(targetFolder, RmNestedFileSystemObject);
                return true;
            }
            return false;
        }
        return true;
    }

    /// <summary>
    /// Deletes the specified directory
    /// </summary>
    /// <param name="compositePath">directory name (not working with paths like folder1/folder2 yet)</param>
    public static void Execute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        //Remove from Dirs
        Dirs.Remove(Dirs.FirstOrDefault(f => f.Name.Equals(compositePath.LastArgName)));

        //Actual directory deletion
        Dirs[offset].Folders.RemoveAll(r => r.Name.Equals(compositePath.LastArgName));
    }

    private static void RmNestedFileSystemObject(IFSO fso)
    {
        CompositePath compositePath = UnpackPath(fso.Path);
        RmCommand.Execute(compositePath);
    }
}
