using CustomCLI.Commands.ICommands;
using static CustomCLI.Kernel;

namespace CustomCLI.Commands;

public class CdCommand : ICommand
{
    /// <summary>
    /// validates if the command can execute
    /// </summary>
    /// <param name="arg">directory(s) name(s) to climb</param>
    /// <returns>true if all the specified directories exist</returns>
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

    /// <summary>
    /// climbs the tree to the desired directory
    /// </summary>
    /// <param name="arg">directory(s) name(s) to climb</param>
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
