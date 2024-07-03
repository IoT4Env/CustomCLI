using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class FdCommand : ICommand
{
    public static bool CanExecute(string arg)
    {
        int deptInt = Convert.ToInt32(arg);
        List<string> dirTree = Tree.Where(w => !string.IsNullOrEmpty(w)).ToList();
        if (dirTree.Count < deptInt)
        {
            Console.WriteLine($"Dept too large for current tree: {string.Join("\\", Tree)}");
            return false;
        }
        return true;
    }

    public static void Execute(string arg)
    {
        int deptInt = Convert.ToInt32(arg);
        while (deptInt-- > 0)
        {
            Tree.RemoveAt(Tree.Count - 1);
            Dept--;
        }
    }
}
