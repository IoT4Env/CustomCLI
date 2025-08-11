namespace CustomCLI.Commands;

public class EchoCommand : ICommand
{
    public static bool CanExecute(string arg)
    {
        return true;
    }

    public static void Execute(string arg) => Console.WriteLine(arg);
}
