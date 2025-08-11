using CustomCLI.Commands;

namespace CustomCLI.CliCommands.Resources;


public class CliCommandsClass : Enumeration<CliCommandsClass>
{
    public static readonly List<CliCommandsClass> CliCommandsList = new()
    {
        new(CliCommandsEnum.Help, "shows all commands available", false, HelpCommand.CheckSyntax),
        new(CliCommandsEnum.Cls, "clears terminal", false, ClsCommand.CheckSyntax),
        new(CliCommandsEnum.Exit, "exits terminal", false, ExitCommand.CheckSyntax),
        new(CliCommandsEnum.Ls, "lists elements in current directory", false, LsCommand.CheckSyntax),
        new(CliCommandsEnum.Echo, "prints stuff on terminal", true, EchoCommand.CheckSyntax),
        new(CliCommandsEnum.Cd, "climb directory(s)", true, CdCommand.CheckSyntax),
        new(CliCommandsEnum.Fd, "fall directory, meaning it descend by a specific amount the fs", true, FdCommand.CheckSyntax),
        new(CliCommandsEnum.Mirror, "Takes the full path of a REAL folder in the PC and creates the structure inside this simulated environment", true, MirrorCommand.CheckSyntax),
        new(CliCommandsEnum.Touch, "creates file", true, TouchCommand.CheckSyntax),
        new(CliCommandsEnum.Rm, "removes file", true, RmCommand.CheckSyntax),
        new(CliCommandsEnum.Mkdir, "creates directory", true, MkDirCommand.CheckSyntax),
        new(CliCommandsEnum.Rmdir, "removes directory", true, RmDirCommand.CheckSyntax),
        new(CliCommandsEnum.Edit, "edits current file", true, EditCommand.CheckSyntax),
        new(CliCommandsEnum.Cat, "Prints file content on the terminal", true, CatCommand.CheckSyntax),
        new(CliCommandsEnum.X3i, "Executes .x3i files which contains the custom cli syntax", true, X3iCommand.CheckSyntax),
        new(CliCommandsEnum.Mv, "Moves file from one folder to another with the following syntax:\n\tmv file.txt->folder", true, MvCommand.CheckSyntax),
        new(CliCommandsEnum.Cp, "Copies the file from one location to another with the following syntax\n\tcp file.txt->folder", true, CpCommand.CheckSyntax),
        new(CliCommandsEnum.Read, "Read a single line from the standard input and stores the value inside a variable", true, ReadCommand.CheckSyntax),
        new(CliCommandsEnum.Expr, "Evaluates the aritmetic operation between two numbers", true, ExprCommand.CheckSyntax),
    };

    private CliCommandsClass(CliCommandsEnum name, string description, bool hasArg, Func<string[], CommandSyntax?> syntax) 
        : base(name, description, hasArg, syntax) { }
}

/// <summary>
/// Simulated cmd commands
/// </summary>
public enum CliCommandsEnum
{
    Help,
    Cls,
    Exit,
    Ls,
    Echo,
    Cd,
    Fd,
    Mirror,
    Touch,
    Rm,
    Mkdir,
    Rmdir,
    Edit,
    Cat,
    X3i,
    Mv,
    Cp,
    Read,
    Expr
}
