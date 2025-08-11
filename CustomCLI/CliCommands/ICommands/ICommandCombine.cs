using CustomCLI.CliCommands.Resources;

namespace CustomCLI.Commands.ICommands;

public interface ICommandCombine
{
    static abstract bool CanExecuteSource(CompositePath source);
    static abstract bool CanExecuteDestination(CompositePath destination);
    static abstract void Execute(CompositePath source, CompositePath destination);
    static abstract CommandSyntax? CheckSyntax(string[] args);
}
