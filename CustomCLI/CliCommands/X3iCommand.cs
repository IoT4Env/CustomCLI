using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands.ICommands;
using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class X3iCommand : ICommandComposite
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
    /// <returns>true if the file exists and if the file extension is correct</returns>
    public static bool CanExecute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        VirtualFile file = GetFileByPosition(compositePath.LastArgName, offset);
        if (file is null)
        {
            Console.WriteLine($"No such file: {compositePath.LastArgName}");
            return false;
        }

        var splittedFile = file.Name.Split('.');
        var fileExtension = splittedFile[splittedFile.Length - 1];
        if (!fileExtension.Equals("x3i"))
        {
            Console.WriteLine($"Unhandled file extension for {CliCommandsEnum.X3i} command: .{fileExtension}");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Executes all lines inside the file as if they where written in the console by the user.
    /// It simulates the bash and sh executables behaviour.
    /// </summary>
    /// <param name="compositePath">file name or path-to-file string</param>
    public static void Execute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        var script = Dirs[offset].Files.FirstOrDefault(r => r.Name.Equals(compositePath.LastArgName));

        var lines = script.Content.Split('\n');
        foreach (var line in lines)
            Kernel.Execute(line.Split(' '));
    }
}
