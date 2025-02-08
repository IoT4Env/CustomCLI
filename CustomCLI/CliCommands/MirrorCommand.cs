using CustomCLI.CliCommands;
using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;
using CustomCLI.FileSystem;
using static CustomCLI.Kernel;
using static CustomCLI.Memory;

namespace CustomCLI.Commands;

public class MirrorCommand : ICommand
{
    private static CommandSyntax syntax = new();
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

        var purifiedArgs = PurifyArgs(args);
        if (!CanResolveVariables(purifiedArgs, out string undefined))
        {
            Console.WriteLine($"{undefined} is not defined");
            return null;
        }
        string[] newArgs = ResolveVariables(purifiedArgs);

        var argumentSyntax = MirrorCommandOption.CheckSyntax(purifiedArgs);
        if (argumentSyntax is null)
        {
            return new CommandSyntax()
            {
                Arg = string.Join(" ", newArgs[0..newArgs.Length])
            };
        }

        return argumentSyntax;
    }

    private static string[] PurifyArgs(string[] args) =>
        args.Where(arg => !string.IsNullOrEmpty(arg)).ToArray();

    /// <summary>
    /// Verifies if the given directory path exists in the REAL PC
    /// </summary>
    /// <param name="arg">Full directory path</param>
    /// <returns>true if directory exists</returns>
    public static bool CanExecute(CommandSyntax syntax)
    {
        if(syntax.Option is not null)
        {
            return false;
        }

        if (!Directory.Exists(syntax.Arg))
        {
            Console.WriteLine($"No such directory: {syntax.Arg}");
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
            string[] splittedPath = directory.Split('\\');
            string dirName = splittedPath[splittedPath.Length - 1];
            GetRealDirs(dirName);

            if (!Directory.EnumerateFileSystemEntries(directory).Any())
                continue;

            syntax.Arg = dirName;
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
    public static void GetRealDirs(string dirName)
    {
        if (dirName is not null)
            Kernel.Execute(new string[] { CliCommandsEnum.Mkdir.ToString(), dirName });
    }

    /// <summary>
    /// Get REAL file names and creates the file inside this simulated environment
    /// </summary>
    /// <param name="filePath">Full file path for file creation</param>
    public static void GetRealFiles(string filePath)
    {
        if (!Path.Exists(filePath))
            return;

        string fileName = Path.GetFileName(filePath);

        string fileContent = File.ReadAllText(filePath);

        Kernel.Execute(new string[] { CliCommandsEnum.Touch.ToString(), fileName });

        VirtualFolder dir = GetCurrentDir();
        dir.Files.FirstOrDefault(f => f.Name.Equals(fileName)).Content = fileContent;
    }
}
