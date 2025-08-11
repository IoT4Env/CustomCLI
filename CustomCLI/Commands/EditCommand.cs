using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;
using System.Text;

namespace CustomCLI.Commands;

public class EditCommand : ICommandComposite
{
    public static bool CanExecute(CompositePath compositePath)
    {
        var offset = Tree.Count + compositePath.ArgsNum - 2;
        VirtualFile file = GetFileByPosition(compositePath.LastArgName, offset);
        if (file is null)
        {
            Console.WriteLine($"No such file: {compositePath.LastArgName}");
            return false;
        }
        return true;
    }

    public static void Execute(CompositePath compositePath)
    {
        ConsoleKeyInfo cki;
        StringBuilder sb = new();

        do
        {
            cki = Console.ReadKey();
            switch (cki.Key)
            {
                case ConsoleKey.Enter:
                    sb.Append('\n');
                    Console.WriteLine();
                    break;
                case ConsoleKey.Spacebar:
                    sb.Append(' ');
                    break;
                case ConsoleKey.Oem4: // ?
                    sb.Append('?');
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine();
                    break;
                case ConsoleKey.OemComma:
                    sb.Append(',');
                    break;
                default:
                    sb.Append(cki.Key.ToString().ToLower());
                    break;
            }
        }
        while (cki.Key != ConsoleKey.Escape);

        CurrentDir dir = GetCurrentDir();
        dir.Files.FirstOrDefault(f => f.Name.Equals(compositePath.LastArgName)).Content = sb.ToString();
    }
}
