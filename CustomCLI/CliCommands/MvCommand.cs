using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;
using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class MvCommand : ICommandCombine
{
    public static CommandSyntax? CheckSyntax(string[] syntax)
    {
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
    /// Executes the combination of both Touch and Rm commands.
    /// Moves the file from the source to the destination
    /// </summary>
    /// <param name="source">File name or path-to-file string</param>
    /// <param name="destination">Folder name or path-to-folder string</param>
    public static void Execute(CompositePath source, CompositePath destination)
    {
        TouchCommand.Execute(source);
        RmCommand.Execute(destination);
    }
}
