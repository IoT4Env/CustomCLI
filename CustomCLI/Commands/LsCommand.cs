using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class LsCommand : ICommand
{
    /// <summary>
    /// Validates if the command can execute
    /// </summary>
    /// <param name="arg">setted by default as an empty string</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(string arg = "") => true;

    /// <summary>
    /// Prints all files and folders inside the current directory
    /// </summary>
    /// <param name="arg">setted by default as an empty string</param>
    public static void Execute(string arg = "")
    {
        VirtualFolder? dir = GetCurrentDir();
        foreach (VirtualFolder folder in dir.Folders)
        {
            Console.ForegroundColor = folder.Color;
            if (folder.Name.Contains(' '))
                Console.Write($"\"{folder.Name}\" ");
            else
                Console.Write($"{folder.Name} ");
        }

        foreach (VirtualFile file in dir.Files)
        {
            Console.ForegroundColor = file.Color;
            if (file.Name.Contains(' '))
                Console.Write($"\"{file.Name}\" ");
            else
                Console.Write($"{file.Name} ");
        }
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine();
    }
}
