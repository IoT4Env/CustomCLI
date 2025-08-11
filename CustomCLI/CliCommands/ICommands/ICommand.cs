namespace CustomCLI.Commands.ICommands;

public interface ICommand
{
    //by declaring static abstract methods we can heredit interfaces on static methods and implement arbitrary code for the method
    //it is not needed to declare a default body for the methods inside the interface.
    static abstract void Execute(CommandSyntax syntax);
    static abstract bool CanExecute(CommandSyntax syntax);
    static abstract CommandSyntax CheckSyntax(string[] syntax);
}
