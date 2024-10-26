using CustomCLI.Commands.ICommands;
using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class FdCommand : ICommand
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
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">number of directories to fall from</param>
    /// <returns>true if the number is valid</returns>
    public static bool CanExecute(CommandSyntax syntax)
    {
        int deptInt = Convert.ToInt32(syntax.Arg);
        List<string> dirTree = Tree.Where(w => !string.IsNullOrEmpty(w)).ToList();
        if (dirTree.Count < deptInt)
        {
            Console.WriteLine($"Dept too large for current tree: {string.Join("\\", Tree)}");
            return false;
        }
        return true;
    }

    public static void Execute(CommandSyntax syntax)
    {
        int deptInt = Convert.ToInt32(syntax.Arg);
        while (deptInt-- > 0)
        {
            Tree.RemoveAt(Tree.Count - 1);
            Dept--;
        }
    }
}
