using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class CdCommand : ICommand
{
    public static bool CanExecute(string arg)
    {
        if(string.IsNullOrEmpty(arg))
            return false;

        var levels = arg.Split('/');
        foreach (var level in levels)
        {
            if (!FolderExists(level))
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
            Dept++;
            Tree.Add(level);
        }
    }
}
