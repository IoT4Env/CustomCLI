using static CustomCLI.Kernel;
namespace CustomCLI.Commands;

public class RmCommand : ICommand
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

    public static bool CheckPathLength(CompositePath compositePath)
    {
        if (compositePath.ArgsNum > 1)
        {
            if (!FolderExists(compositePath.Folders))
            {
                Console.WriteLine($"No such folder: {compositePath.Folders}");
                return false;
            }
            return true;
        }
        return false;
    }
}
