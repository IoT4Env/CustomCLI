using static CustomCLI.Kernel;
namespace CustomCLI.Commands;

public class RmCommand : ICommand, ICheckPath
{
    public static bool CanExecute(string arg)
    {
        if (!FileExists(arg))
        {
            Console.WriteLine($"No such file: {arg}");
            return false;
        }
        return true;
    }

    public static void Execute(string arg) => Dirs[Dept].Files.RemoveAll(r => r.Name.Equals(arg));

    public static bool CheckPathLength(int argsNum) => argsNum > 1;
}
