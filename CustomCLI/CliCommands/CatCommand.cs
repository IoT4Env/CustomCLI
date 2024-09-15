using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;
using CustomCLI.CliCommands.Resources;

namespace CustomCLI.Commands;

public class CatCommand : ICommandComposite
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
    /// Tracks the specific file present somewhere in the file system and checks if the file exists
    /// </summary>
    /// <param name="compositePath">file name or path-to-file string</param>
    /// <returns>execution permission</returns>
    public static bool CanExecute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        VirtualFile dir = GetFileByPosition(compositePath.LastArgName, offset);

        if (dir is null)
        {
            Console.WriteLine($"No such file: {compositePath.LastArgName}");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Prints the content of the specified file
    /// </summary>
    /// <param name="compositePath">file name or path-to-file string</param>
    public static void Execute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        var file = Dirs[offset].Files.FirstOrDefault(r => r.Name.Equals(compositePath.LastArgName));
        Console.WriteLine(file.Content);
    }
}
