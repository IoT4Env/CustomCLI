using CustomCLI.Commands;

namespace CustomCLI.CliCommands.Resources;


public class CliCommandsClass : Enumeration<CliCommandsClass>
{
    public static readonly List<CliCommandsClass> CliCommandsList = new()
    {
        new(CliCommandsEnum.Help.ToString(), "shows all commands available", false, HelpCommand.CheckSyntax),
    };
    //public static readonly CliCommandsClass Clc = new("shows all commands available", false, HelpCommand.CheckSyntax);

    private CliCommandsClass(string name, string description, bool hasArg, Action<CommandSyntax> syntax) 
        : base(name, description, hasArg, syntax) { }
}

/// <summary>
/// Simulated cmd commands
/// </summary>
public enum CliCommandsEnum
{
    Help,
    //[Descriptor("clears terminal", false)]
    Cls,
    //[Descriptor("exits terminal", false)]
    Exit,
    //[Descriptor("lists elements in current directory", false)]
    Ls,
    //[Descriptor("prints stuff on terminal", true)]
    Echo,
    //[Descriptor("climb directory(s)", true)]
    Cd,
    //[Descriptor("fall directory, meaning it descend by a specific amount the fs", true)]
    Fd,
    //[Descriptor("Takes the full path of a REAL folder in the PC and creates the structure inside this simulated environment", true)]
    Mirror,
    //[Descriptor("creates file", true)]
    Touch,
    //[Descriptor("removes file", true)]
    Rm,
    //[Descriptor("creates folder", true)]
    Mkdir,
    //[Descriptor("removes directory if empty", true)]
    Rmdir,
    //[Descriptor("edits current file", true)]
    Edit,
    //[Descriptor("Prints file content on the terminal", true)]
    Cat,
    //[Descriptor("Executes .x3i files which contains the custom cli syntax", true)]
    X3i,
    //[Descriptor("Moves file from one folder to another with the following syntax:\n\tmv file.txt->folder", true)]
    Mv,
    //[Descriptor("Copies the file from one location to another with the following syntax\n\tcp file.txt->folder", true)]
    Cp,
}


///// <summary>
///// Custom class to extend enum attributes
///// </summary>
//public class Descriptor : Attribute
//{
//    public string Description { get; }
//    public bool HasArg { get; }
//    public Action<CommandSyntax> CheckSyntax { get; }

//    public Descriptor(string descriptor, bool hasArg, Action<CommandSyntax> checkSyntax)
//    {
//        Description = descriptor;
//        HasArg = hasArg;
//        CheckSyntax = checkSyntax;
//    }
//}

///// <summary>
///// Extends the functionality of CliCommands enum to get the element deescription
///// </summary>
//public static class CliCommandsExtension
//{
//    /// <summary>
//    /// Handle the description decoprator
//    /// </summary>
//    /// <param name="command"></param>
//    /// <returns>Command description</returns>
//    public static string ToDescriptionString(this CliCommandsEnum command)
//    {
//        Descriptor[] attributes = (Descriptor[])command
//           .GetType()
//           .GetField(command.ToString())
//           .GetCustomAttributes(typeof(Descriptor), true);
//        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
//    }

//    /// <summary>
//    /// Handle the HasArg decoprator
//    /// </summary>
//    /// <param name="command"></param>
//    /// <returns>The possibility for the command to have an argument</returns>
//    public static bool ToHasArgBool(this CliCommandsEnum command)
//    {
//        Descriptor[] attributes = (Descriptor[])command
//           .GetType()
//           .GetField(command.ToString())
//           .GetCustomAttributes(typeof(Descriptor), true);
//        return attributes.Length > 0 && attributes[0].HasArg;
//    }

//    public static Action<CommandSyntax> ToCheckSyntax(this CliCommandsEnum command)
//    {
//        Descriptor[] attributes = (Descriptor[])command
//           .GetType()
//           .GetField(command.ToString())
//           .GetCustomAttributes(typeof(Descriptor), true);
//        return attributes[0].CheckSyntax;
//    }
//}
