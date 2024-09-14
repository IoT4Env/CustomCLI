using static CustomCLI.Kernel;
using CustomCLI.Commands.ICommands;
using System.Text;
using CustomCLI.CliCommands.Resources;

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
        char key;
        StringBuilder sb = new();

        do
        {
            key = Console.ReadKey().KeyChar;

            //bitwise operator to check for the \r char (just to speed up the check for special keys like "enter" on every key press)
            //shortly, we are checking if key bits are complementary to 0x0D with the bitwise XOR operator
            //Just found out the power of conditional printing!
            //Might come useful in other cases
            //Console.Write(((key ^ 0x0D) == 0x00) ? '\n' //if is enter
            //            : ((key ^ 0x08) == 0x00) ? " \b"//if is delete
            //            : "");//if is canc

            //On future features:
            //Add cursor movement and possibility to reapon an existing file content.
            //also, try to remove duplice esc key pressed check
            switch (key)
            {
                case '\x0D'://if is enter
                    Console.WriteLine();
                    sb.Append('\n');//the returned key is \r, which is not the correct EOL char for a file
                    break;
                case '\x08'://if is delete
                    Console.Write(" \b");
                    sb.Remove(sb.Length - 1, 1);
                    break;
                case '\u001b'://need to check if esc twice to avoid the esc character to be inserted in the string builder
                    break;
                default:
                    sb.Append(key);
                    break;
            }
        }
        while (key != '\u001b');//esc is \u001b

        VirtualFolder dir = GetCurrentDir();
        dir.Files.FirstOrDefault(f => f.Name.Equals(compositePath.LastArgName)).Content = sb.ToString();
    }
}
