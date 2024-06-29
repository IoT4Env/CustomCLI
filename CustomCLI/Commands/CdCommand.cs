namespace CustomCLI.Commands;

public class CdCommand : ICommand
{
    public static bool CanExecute(string arg)
    {
        var levels = arg.Split('/');
        foreach (var level in levels)
        {
            if (!Kernel.FolderExists(level))
            {
                Console.WriteLine($"No such directory: {level}");
                return false;
            }
        }
        return true;
    }

    public static void Execute(string arg)
    {
        var levels = arg.Split('/');
        foreach (var level in levels)
        {
            Kernel.Dept++;
            Kernel.Tree.Add(level);
        }
    }
}
