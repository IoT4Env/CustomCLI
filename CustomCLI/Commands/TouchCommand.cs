using CustomCLI.Commands.ICommands;
using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class TouchCommand : ICommandComposite
{
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

        if (Enum.TryParse<FileExtension>(splittedArg[splittedArg.Length - 1], ignoreCase: true, out var extension))
        {
            ConsoleColor color = 0;

            switch (extension)
            {
                case FileExtension.Txt:
                    color = (ConsoleColor)FileExtension.Txt;
                    break;
                case FileExtension.Cs:
                    color = (ConsoleColor)FileExtension.Cs;
                    break;
                case FileExtension.Zip:
                    color = (ConsoleColor)FileExtension.Zip;
                    break;
                case FileExtension.Exe:
                    color = (ConsoleColor)FileExtension.Exe;
                    break;
                case FileExtension.X3i:
                    color = (ConsoleColor)FileExtension.X3i;
                    break;
            }

            var offset = Tree.Count + compositePath.ArgsNum - 2;
            Dirs[offset].Files.Add(new VirtualFile
            {
                Color = color,
                Name = compositePath.LastArgName,
                Content = string.Empty
            });
        }
    }
}
