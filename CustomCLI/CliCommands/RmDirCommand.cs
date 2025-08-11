using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;
using CustomCLI.CliCommands.Resources;

namespace CustomCLI.Commands;

public class RmDirCommand : ICommandComposite
{
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
            //TODO:
            //Generate script that deletes all files and folders inside directory if the user wants to
            Console.WriteLine($"\n{compositePath.LastArgName} contains the following elements:");

            BrowseDirectory(targetFolder, EchoCommand.EchoFileName, EchoCommand.EchoFolderName);

            Console.WriteLine("Would you like to remove them? [y/N]");
            string? response = Console.ReadLine();
            if (response.Equals("y"))
            {
                BrowseDirectory(targetFolder, RmNestedFile, RmNestedFolder);
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
        var a = Dirs[offset].Folders;
        //Remove from Dirs
        Dirs.Remove(Dirs.FirstOrDefault(f => f.Name.Equals(compositePath.LastArgName)));

        var b = Dirs[offset].Folders;

        //Actual directory deletion
        Dirs[offset].Folders.RemoveAll(r => r.Name.Equals(compositePath.LastArgName));
    }

    private static void RmNestedFile(VirtualFile file)
    {
        //string fullPath = $"{string.Join('/', Tree)}/{file.Name}";
        CompositePath compositePath = UnpackPath(file.Path);
        RmCommand.Execute(compositePath);
    }

    private static void RmNestedFolder(VirtualFolder folder)
    {
        CompositePath compositePath = UnpackPath(folder.Path);
        Execute(compositePath);
    }
}
