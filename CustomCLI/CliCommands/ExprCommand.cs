using CustomCLI.Commands.ICommands;
using System.Linq;
using static CustomCLI.Memory;

namespace CustomCLI.CliCommands;

public class ExprCommand : ICommand
{
    private static string Operators { get; set; } = "+-*/";
    public static bool CanExecute(CommandSyntax syntax)
    {
        var arg = syntax.Arg.Split(" ");
        bool isValidExpression = 
               int.TryParse(arg[0], out _)
               && Operators.Contains(arg[1])
               && int.TryParse(arg[2], out _);

        if (!isValidExpression)
        {
            Console.WriteLine("Could not parse numbers or operator");
        }

        return isValidExpression;
    }

    public static CommandSyntax? CheckSyntax(string[] args)
    {
        if(args.Length == 0)
        {
            Console.WriteLine("Argument required");
            return null; 
        }

        if(args.Length > 3)
        {
            Console.WriteLine("Argument exceeded");
            return null;
        }

        if(args.Length != 3)
        {
            Console.WriteLine("Expected expression like \"num1 operator num2\"");
            return null;
        }

        if (!CanResolveVariables(args, out string undefined))
        {
            Console.WriteLine($"{undefined} is not defined");
            return null;
        }

        string[] newArgs = ResolveVariables(args);

        return new CommandSyntax()
        {
            Arg = string.Join(" ", newArgs[0..newArgs.Length])
        };
    }

    public static void Execute(CommandSyntax syntax)
    {
        var args = syntax.Arg.Split(" ");
        int arg1 = Convert.ToInt32(args[0]);
        int arg2 = Convert.ToInt32(args[2]);

        int result = 0;
        switch (args[1])
        {
            case "+":
                result = arg1 + arg2;
                break;
            case "-":
                result = arg1 - arg2;
                break;
            case "*":
                result = arg1 * arg2;
                break;
            case "/":
                result = arg1 / arg2;
                break;
            default:
                break;
        }
        Console.WriteLine(result);
    }
}
