namespace CustomCLI.Commands;

public interface ICommand
{
    //by declaring static abstract methods we can heredit interfaces on static methods and implement arbitrary code for the method
    //it is not needed to declare a default body for the methods inside the interface.
    static abstract void Execute(string arg);
    static abstract bool CanExecute(string arg);
}
