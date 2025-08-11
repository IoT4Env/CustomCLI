using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;
using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class TouchCommand : ICommandComposite
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
    /// <returns>create permission</returns>
    public static bool CanExecute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        VirtualFile file = GetFileByPosition(compositePath.LastArgName, offset);
        if (file is not null)
        {
            Console.WriteLine($"File already exists: {compositePath.LastArgName}");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Creates a file in the desired location specified inside the path
    /// File extension is recognized with different colors
    /// </summary>
    /// <param name="compositePath">file name or path-to-create-file string</param>
    public static void Execute(CompositePath compositePath)
    {
        var splittedArg = compositePath.LastArgName.Split('.');

        if (Enum.TryParse<FileExtensions>(splittedArg[splittedArg.Length - 1], ignoreCase: true, out var extension))
        {
            ConsoleColor color = (ConsoleColor)Enum.GetValues<FileExtensions>()//Get all values from the FileExtension Enum
                .FirstOrDefault(f => (int)extension == (int)f);//Extract the color where the extension int value equals one of the enum int value

            var offset = Tree.Count + compositePath.ArgsNum - 2;
            var foldersCheck = compositePath.Folders.Equals(string.Empty) ? "" : $"{compositePath.Folders}/";
            Dirs[offset].Files.Add(new VirtualFile
            {
                Color = color,
                Name = compositePath.LastArgName,
                //join the position of the command execution, the folders provided to the argument, and the file name
                Path = $"{string.Join('/', Tree)}/{foldersCheck}{compositePath.LastArgName}",
                Content = string.Empty,
                Extension = extension
            });
        }
    }
}
