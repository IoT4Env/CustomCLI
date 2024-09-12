using System.ComponentModel;

namespace CustomCLI;

/// <summary>
/// Simulated cmd commands
/// </summary>
public enum CliCommands
{
    [Description("shows all commands available")]
    Help,
    [Description("clears terminal")]
    Cls,
    [Description("exits terminal")]
    Exit,
    [Description("prints stuff on terminal")]
    Echo,
    [Description("climb directory(s)")]
    Cd,
    [Description("fall directory, meaning it descend by a specific amount the fs")]
    Fd,
    [Description("Takes the full path of a REAL folder in the PC and creates the structure inside this simulated environment")]
    Mirror,
    [Description("creates file")]
    Touch,
    [Description("removes file")]
    Rm,
    [Description("creates folder")]
    Mkdir,
    [Description("removes directory if empty")]
    Rmdir,
    [Description("lists elements in current directory")]
    Ls,
    [Description("edits current file")]
    Edit,
    [Description("Prints file content on the terminal")]
    Cat,
    [Description("Executes .x3i files which contains the custom cli syntax")]
    X3i,
    [Description("Moves file from one folder to another with the following syntax:\n\tmv file.txt->folder")]
    Mv,
    [Description("Copies the file from one location to another with the following syntax\n\tcp file.txt->folder")]
    Cp,
}

public static class CliCommandsExtension
{

    /// <summary>
    /// Extends the functionality of CliCommands enum to get the element deescription
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Description of the CliCommand</returns>    
    public static string ToDescriptionString(this CliCommands command)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])command
           .GetType()
           .GetField(command.ToString())
           .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}