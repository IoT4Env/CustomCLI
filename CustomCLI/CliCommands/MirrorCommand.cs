using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class MirrorCommand : ICommand
{
    private static CommandSyntax syntax = new();
    public static CommandSyntax? CheckSyntax(string[] syntax)
    {
        return null;
    }

    /// <summary>
    /// Verifies if the given directory path exists in the REAL PC
    /// </summary>
    /// <param name="arg">Full directory path</param>
    /// <returns>true if directory exists</returns>
    public static bool CanExecute(CommandSyntax syntax)
    {
        string? directory = Path.GetDirectoryName(syntax.Arg);
        if (!Path.Exists(syntax.Arg))
        {
            Console.WriteLine($"No such directory: {directory}");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Executes the command for mirroring all folders and files inside this simulated environment
    /// Surround the path with doudle quotes (") if it contains spaces
    /// </summary>
    /// <param name="arg">Full directory path</param>
    public static void Execute(CommandSyntax syntax)
    {
        string[] rootFolders = Directory.GetDirectories(syntax.Arg);
        string[] rootFiles = Directory.GetFiles(syntax.Arg);
        
        foreach (string file in rootFiles)
            GetRealFiles(file);

        //works even with space-defined paths!
        BrowseDirectories(rootFolders);
    }

    /// <summary>
    /// Browse all elements inside the given REAL directory path
    /// </summary>
    /// <param name="directories">Full directory names to browse</param>
    private static void BrowseDirectories(string[] directories)
    {
        foreach (string directory in directories)
        {
            GetRealDirs(directory);

            if (!Directory.EnumerateFileSystemEntries(directory).Any())
                continue;

            syntax.Arg = directory;
            CdCommand.Execute(syntax);

            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
                GetRealFiles(file);

            BrowseDirectories(Directory.GetDirectories(directory));

            syntax.Arg = "1";
            FdCommand.Execute(syntax);
        }
    }

    /// <summary>
    /// Get REAL directory names and creates the folder inside this simulated environment
    /// </summary>
    /// <param name="dirPath">Full directory path for folder creation</param>
    private static void GetRealDirs(string dirPath)
    {
        string[] splittedPath = dirPath.Split('\\');
        string dirName = splittedPath[splittedPath.Length - 1];

        if (dirName is not null)
            Kernel.Execute(new string[] { CliCommandsEnum.Mkdir.ToString(), dirName });
    }

    /// <summary>
    /// Get REAL file names and creates the file inside this simulated environment
    /// </summary>
    /// <param name="filePath">Full file path for file creation</param>
    private static void GetRealFiles(string filePath)
    {
        string? fileName = Path.GetFileName(filePath);

        if (fileName is not null)
            Kernel.Execute(new string[] { CliCommandsEnum.Touch.ToString(), fileName });
    }
}
