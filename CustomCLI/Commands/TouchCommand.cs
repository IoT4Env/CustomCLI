using CustomCLI.Commands.ICommands;
using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class TouchCommand : ICommandComposite
{
    public static bool CanExecute(CompositePath compositePath)
    {
        VirtualFile file = GetFileByPosition(compositePath.LastArgName, compositePath.ArgsNum - 1);
        if (file is not null)
        {
            Console.WriteLine($"File already exists: {compositePath.LastArgName}");
            return false;
        }
        return true;
    }

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
            }

            Dirs[compositePath.ArgsNum - 1].Files.Add(new VirtualFile
            {
                Color = color,
                Name = compositePath.LastArgName,
                Content = string.Empty
            });
        }

    }
}
