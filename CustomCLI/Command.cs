namespace CustomCLI;

public class Command<Type>
{
    public Type CommandName { get; set; }
    public CommandSyntax Syntax { get; set; }
}
