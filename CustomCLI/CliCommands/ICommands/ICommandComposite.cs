using CustomCLI.CliCommands.Resources;

namespace CustomCLI.Commands.ICommands;

public interface ICommandComposite
{
    #region ICommandComposite
    /*
    For commands that can interact with files and folders outside the folder where the command is written,
    this interface is used.
     */
    #endregion
    static abstract bool CanExecute(CompositePath compositePath);
    static abstract void Execute(CompositePath compositePath);
}
