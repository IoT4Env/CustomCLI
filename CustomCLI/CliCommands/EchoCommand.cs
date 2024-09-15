using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;

namespace CustomCLI.Commands;

public class EchoCommand : ICommand
{
    public static CommandSyntax? CheckSyntax(string[] args)
    {
        //If no arguments where given
        if (args.Length == 1)
        {
            Console.WriteLine("Argument required");
        }
        else if (args.Length == 2)
        {
            return new CommandSyntax()
            {
                Arg = args[1]
            };
        }
        Console.WriteLine("Arguments excedeed");
        return null;
    }

    /// <summary>
    /// A Dictionary with all options associated with this command
    /// </summary>
    private static readonly Dictionary<string, Action<string>> _options = new()
    { 
        { "-e", EOpt },
        { string.Empty, Echo}//default value
    };
    /// <summary>
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">string to be displayed on the terminal</param>
    /// <returns>always return true</returns>
    public static bool CanExecute(CommandSyntax syntax) => true;

    /// <summary>
    /// Print the specified string on the terminal.
    /// Use the -e option to remove all 'e' from the argument.
    /// </summary>
    /// <param name="arg">string to be dislayed on the terminal</param>
    public static void Execute(CommandSyntax syntax) =>
        _options[syntax.Option].Invoke(syntax.Arg);

    private static void Echo(string arg) => Console.WriteLine(arg);
    private static void EOpt(string opt) => Console.WriteLine(opt.Replace("e", ""));

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
