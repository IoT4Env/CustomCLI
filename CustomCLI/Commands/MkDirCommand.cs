using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class MkDirCommand : ICommandComposite
{
    /// <summary>
    /// Tracks the specific directory present somewhere in the file system and checks if it already exists
    /// </summary>
    /// <param name="compositePath">directory name</param>
    /// <returns>create permission</returns>
    public static bool CanExecute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;//subtract 2 because of 2 lengths (not indexes) added togeter
        CurrentDir dir = GetDirectoryByPosition(compositePath.LastArgName, offset);

        if (dir is not null)
        {
            Console.WriteLine($"Directory {compositePath.LastArgName} already exists");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Creates the specified directory
    /// </summary>
    /// <param name="compositePath">directory name (not working with paths like folder1/folder2 yet)</param>
    public static void Execute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        Dirs[offset].Folders.Add(new VirtualFolder
        {
            Color = ConsoleColor.Blue,
            Name = compositePath.LastArgName
        });

        Dirs.Add(new CurrentDir
        {
            Name = compositePath.LastArgName,
            Dept = offset
        });
    }
}
