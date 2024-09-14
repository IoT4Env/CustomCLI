using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class EchoCommand : ICommand
{
    /// <summary>
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">string to be displayed on the terminal</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(CommandSyntax syntax) => true;

    /// <summary>
    /// print the specified string on the terminal
    /// </summary>
    /// <param name="arg">string to be dislayed on the terminal</param>
    public static void Execute(CommandSyntax syntax) => Console.WriteLine(syntax.Arg[0]);

    public static void EchoFileName(VirtualFile file)
    {
        Console.ForegroundColor = file.Color;
        Console.Write(file.Name.Contains(' ') 
            ? $"\"{file.Name}\" " 
            : $"{file.Name} ");
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void EchoFolderName(VirtualFolder folder)
    {
        Console.ForegroundColor = folder.Color;
        Console.Write(folder.Name.Contains(' ')
            ? $"\"{folder.Name}\" "
            : $"{folder.Name} ");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
