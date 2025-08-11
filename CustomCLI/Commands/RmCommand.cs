using static CustomCLI.Kernel;
namespace CustomCLI.Commands;

public class RmCommand : ICommandComposite
{
    public static bool CanExecute(CompositePath compositePath)
    {
        VirtualFile file = GetFileByPosition(compositePath.LastArgName, compositePath.ArgsNum - 1);
        if (file is null)
        {
            Console.WriteLine($"No such file: {compositePath.LastArgName}");
            return false;
        }
        return true;
    }
        
    public static void Execute(CompositePath compositePath) =>
        Dirs[compositePath.ArgsNum - 1].Files.RemoveAll(r => r.Name.Equals(compositePath.LastArgName));
}
