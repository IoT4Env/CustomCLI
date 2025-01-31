using CustomCLI.Commands.ICommands;
using CustomCLI.FileSystem;
using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class CpCommand : ICommandCombine
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
    /// Checks if the given source file exists
    /// </summary>
    /// <param name="source">File name or path-to-file string</param>
    /// <returns>true if the specified file exists</returns>
    public static bool CanExecuteSource(CompositePath source)
    {
        var offset = Tree.Count + source.ArgsNum - 2;
        VirtualFile file = GetFileByPosition(source.LastArgName, offset);
        if (file is null)
        {
            Console.WriteLine($"No such file: {source.LastArgName}");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if the given folder exists
    /// </summary>
    /// <param name="destination">Folder name or path-to-folder string</param>
    /// <returns>true if the specifid folder exists</returns>
    public static bool CanExecuteDestination(CompositePath destination)
    {
        var offset = Tree.Count + destination.ArgsNum - 2;
        VirtualFolder folder = GetFolderByPosition(destination.LastArgName, offset);
        if (folder is null)
        {
            Console.WriteLine($"No such folder: {destination.LastArgName}");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Contructs the path for the file to be created and creates the file.
    /// The TouchCommand logic is used to create the file.
    /// </summary>
    /// <param name="source">File name or path-to-file string</param>
    /// <param name="destination">Folder name or path-to-folder string</param>
    public static void Execute(CompositePath source, CompositePath destination)
    {
        var fileToCreate = UnpackPath($"{destination.FullPath}/{source.LastArgName}");
        TouchCommand.Execute(fileToCreate);
    }
}
