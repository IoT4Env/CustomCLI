using CustomCLI.CliCommands.Resources;
using CustomCLI.Commands;

namespace CustomCLI;

public class Kernel
{
    public static List<VirtualFolder> Dirs { get; set; } = new List<VirtualFolder> { new VirtualFolder { Name = string.Empty, Dept = -1 } };
    public static bool IsExit { get; set; } = false;
    public static int Dept { get; set; } = 0;
    public static List<string> Tree { get; set; } = new() { string.Empty };

    //when "installing" new commands, the name should be added here
    private static List<string> ExternalCommands { get; set; } = new() { };

    //For each command type, a new folder has to be added
    private static readonly Dictionary<CliCommandsClass, Action<CommandSyntax>> CliCommandsDict2 = new()
    {
        { CliCommandsClass.Help, Help },
        //{ CliCommandsClass.Cls, Cls },
        //{ CliCommandsClass.Ls, Ls },
        //{ CliCommandsClass.Exit, Exit },
        //{ CliCommandsClass.Echo, Echo },//arg => Echo(arg)
        //{ CliCommandsClass.Cd, Cd },
        //{ CliCommandsClass.Fd, Fd },
        //{ CliCommandsClass.Touch, Touch },
        //{ CliCommandsClass.Rm, Rm },
        //{ CliCommandsClass.Mkdir, MkDir },
        //{ CliCommandsClass.Rmdir, Rmdir },
        //{ CliCommandsClass.Edit, Edit },
        //{ CliCommandsClass.Cat, Cat },
        //{ CliCommandsClass.X3i, X3i },
        //{ CliCommandsClass.Mv, Mv },
        //{ CliCommandsClass.Cp, Cp },
        //{ CliCommandsClass.Mirror, Mirror },

    };


    private static Dictionary<CliCommandsEnum, Action<CommandSyntax>> CliCommandsDict = new()
    {
        { CliCommandsEnum.Help, Help },
        { CliCommandsEnum.Cls, Cls },
        { CliCommandsEnum.Ls, Ls },
        { CliCommandsEnum.Exit, Exit },
        { CliCommandsEnum.Echo, Echo },//arg => Echo(arg)
        { CliCommandsEnum.Cd, Cd },
        { CliCommandsEnum.Fd, Fd },
        { CliCommandsEnum.Touch, Touch },
        { CliCommandsEnum.Rm, Rm },
        { CliCommandsEnum.Mkdir, MkDir },
        { CliCommandsEnum.Rmdir, Rmdir },
        { CliCommandsEnum.Edit, Edit },
        { CliCommandsEnum.Cat, Cat },
        { CliCommandsEnum.X3i, X3i },
        { CliCommandsEnum.Mv, Mv },
        { CliCommandsEnum.Cp, Cp },
        { CliCommandsEnum.Mirror, Mirror },
    };

    /// <summary>
    /// Executes command based on user input
    /// </summary>
    /// <param name="args"></param>
    public static void Execute(string[] args)
    {
        if (args[0].Equals(string.Empty))
            return;

        //add as many if as of how many commands types there are
        //CliCommands type, Docker type, Python type, and so on...
        if (Enum.TryParse<CliCommandsClass>(args[0], ignoreCase: true, out var command2))
        {

        }

        if (Enum.TryParse<CliCommandsEnum>(args[0], ignoreCase: true, out var command))
        {
            if (CliCommandsDict.TryGetValue(command, out Action<CommandSyntax>? method))
            {
                command.ToCheckSyntax();
                CheckSyntax(args, method);
                return;
            }
        }
        Console.WriteLine($"No such command: {args[0]}");
    }

    private static void CheckSyntax(string[] args, Action<CommandSyntax> commandExec)
    {
        //re re re simplify this method...
        //in the future, the check syntax can be applied inside each commands instead of a general way to write the command
        //this should add more flexibility on the amount of arguments and options for the specific command
        args = args.Where(w => !string.IsNullOrEmpty(w)).ToArray();

        CommandSyntax syntax = new()
        {
            Arg = string.Empty,
            Option = string.Empty,
        };

        if (args.Length == 1)
        {
            commandExec.Invoke(syntax);
            return;
        }

        if (args.Length == 2 && args[1][0].Equals('-'))
        {
            syntax.Option = args[1];
            commandExec.Invoke(syntax);
            return;
        }

        string arguments = string.Join(' ', args[1..args.Length]);

        if (args[1][0].Equals('-'))
        {
            syntax.Option = args[1];
            arguments = string.Join(' ', args[2..args.Length]);
        }

        //suppose the remaining args are the given command arguments
        if (arguments.Where(s => s.Equals('"')).Count() is 2 or 0)
        {
            string quotedString = string.Join(' ', arguments).Replace("\"", "");
            syntax.Arg = quotedString;
            commandExec.Invoke(syntax);//pass syntax here
            return;
        }
        Console.WriteLine("Missing char: \"");
    }

    #region Query file system

    /// <summary>
    /// Gets the directory where the command is written
    /// </summary>
    /// <returns>directory object where the command is written</returns>
    public static VirtualFolder GetCurrentDir() =>
        Dirs.FirstOrDefault(d => d.Name.Equals(Tree[Tree.Count - 1]) && d.Dept == Dept - 1);

    /// <summary>
    /// Gets specific folder based on name and dept
    /// </summary>
    /// <param name="folder">folder name</param>
    /// <param name="dept">dept number</param>
    /// <returns>directory object identifid in the file system</returns>
    public static VirtualFolder GetDirectoryByPosition(string folder, int dept) =>
        Dirs.FirstOrDefault(d => d.Name.Equals(folder) && d.Dept == dept);

    /// <summary>
    /// Gets specific file based on name and dept
    /// </summary>
    /// <param name="file">file name</param>
    /// <param name="dept">dept number</param>
    /// <returns>virtual file object identifid in the file system</returns>
    public static VirtualFile GetFileByPosition(string file, int dept) =>
        Dirs[dept].Files.FirstOrDefault(f => f.Name.Equals(file));

    /// <summary>
    /// Gets specific folder based on name and dept
    /// </summary>
    /// <param name="folder">folder name</param>
    /// <param name="dept">dept number</param>
    /// <returns>virtual folder object identified in the file system</returns>
    public static VirtualFolder GetFolderByPosition(string folder, int dept) =>
        Dirs[dept].Folders.FirstOrDefault(f => f.Name.Equals(folder));

    /// <summary>
    /// Get file by name inside the directory where the command is written
    /// </summary>
    /// <param name="file">file name</param>
    /// <returns>file object identifid in the folder where the command is written</returns>
    public static VirtualFile GetVirtualFile(string file) =>
        GetCurrentDir().Files.FirstOrDefault(f => f.Name.Equals(file));

    /// <summary>
    /// Checks if the specifid directory is empty
    /// </summary>
    /// <param name="dir">object directory</param>
    /// <returns>true if folder is empty, false other whise</returns>
    public static bool IsFolderEmpty(VirtualFolder dir) =>
        dir.Folders.Count == 0 && dir.Files.Count == 0;

    /// <summary>
    /// Checks if a specific file exists inside the folder where the command is written
    /// </summary>
    /// <param name="name">file name</param>
    /// <returns>true if file exists, false other whise</returns>
    public static bool FileExists(string name)
    {
        VirtualFolder? dir = GetCurrentDir();
        return dir.Files.Where(file => file.Name.Equals(name)).Count() != 0;
    }

    /// <summary>
    /// Checks if a specific folder exists inside the folder where the command is written
    /// </summary>
    /// <param name="name">folder name</param>
    /// <returns>true if folder exists, false other whise</returns>

    public static bool FolderExists(string name)
    {
        VirtualFolder? dir = GetCurrentDir();
        name = name.Replace("/", "");
        return dir.Folders.Where(f => f.Name.Equals(name)).Count() != 0;
    }

    public static void BrowseDirectory(VirtualFolder targetFolder, Action<VirtualFile> fileAction, Action<VirtualFolder> folderAction)
    {
        var mutableFileCount = targetFolder.Files.Count;
        for (int i = 0; i < mutableFileCount; i++)
        {
            if (mutableFileCount != targetFolder.Files.Count)
                mutableFileCount = targetFolder.Files.Count;
            fileAction.Invoke(targetFolder.Files[i]);
        }

        var mutableFolderCount = targetFolder.Folders.Count;
        for (int i = 0; i < mutableFolderCount; i++)
        {
            if (mutableFolderCount != targetFolder.Folders.Count)
                mutableFolderCount = targetFolder.Folders.Count;
            folderAction.Invoke(targetFolder.Folders[i]);

            //already checkd if the list has beed changed
            if (i <= mutableFolderCount)
                continue;

            if (!IsFolderEmpty(targetFolder.Folders[i]))
                BrowseDirectory(targetFolder.Folders[i], fileAction, folderAction);
        }
        Console.WriteLine();
    }
    #endregion

    /// <summary>
    /// Extracts arguments inside a composite path.
    /// EG:
    /// a path constructed like folder/file.txt
    /// extracts the following informations:
    /// ArgsNum = components of composite path
    /// LastArgIndex = index of last components of composite path
    /// LastArgName = name of last components of composite path
    /// Folders = path-to-file
    /// </summary>
    /// <param name="arg">composite path argument</param>
    /// <returns>a CompositePath object</returns>
    public static CompositePath UnpackPath(string arg)
    {
        string[] args = arg.Split('/');

        //chech if the path begins with '/'
        if (args[0].Equals(string.Empty))
            args = args[1..args.Length];
        int fileIndex = args.Length - 1;
        return new CompositePath()
        {
            ArgsNum = args.Length,
            LastArgIndex = fileIndex,
            LastArgName = args[fileIndex],
            Folders = string.Join('/', args.Take(fileIndex)),
            FullPath = arg
        };
    }

    #region Commands

    private static void Help(CommandSyntax syntax) => HelpCommand.Execute(syntax);

    private static void Cls(CommandSyntax syntax) => ClsCommand.Execute(syntax);

    private static void Ls(CommandSyntax syntax) => LsCommand.Execute(syntax);

    private static void Exit(CommandSyntax syntax) => IsExit = true;

    private static void Echo(CommandSyntax syntax) => EchoCommand.Execute(syntax);

    private static void Cd(CommandSyntax syntax)
    {
        if (CdCommand.CanExecute(syntax))
            CdCommand.Execute(syntax);
    }

    private static void Fd(CommandSyntax syntax)
    {
        if (FdCommand.CanExecute(syntax))
            FdCommand.Execute(syntax);
    }

    private static void Mirror(CommandSyntax syntax)
    {
        if (MirrorCommand.CanExecute(syntax))
            MirrorCommand.Execute(syntax);
    }

    private static void Touch(CommandSyntax syntax)
    {
        CompositePath compositePath = UnpackPath(syntax.Arg);

        if (TouchCommand.CanExecute(compositePath))
            TouchCommand.Execute(compositePath);
    }

    private static void Rm(CommandSyntax syntax)
    {
        CompositePath compositePath = UnpackPath(syntax.Arg);

        if (RmCommand.CanExecute(compositePath))
            RmCommand.Execute(compositePath);
    }

    private static void MkDir(CommandSyntax syntax)
    {
        CompositePath compositePath = UnpackPath(syntax.Arg);

        if (MkDirCommand.CanExecute(compositePath))
            MkDirCommand.Execute(compositePath);
    }

    private static void Rmdir(CommandSyntax syntax)
    {
        CompositePath compositePath = UnpackPath(syntax.Arg);

        if (RmDirCommand.CanExecute(compositePath))
            RmDirCommand.Execute(compositePath);
    }

    private static void Edit(CommandSyntax syntax)
    {
        CompositePath compositePath = UnpackPath(syntax.Arg);

        if (EditCommand.CanExecute(compositePath))
            EditCommand.Execute(compositePath);
    }

    private static void Cat(CommandSyntax syntax)
    {
        CompositePath compositePath = UnpackPath(syntax.Arg);

        if (CatCommand.CanExecute(compositePath))
            CatCommand.Execute(compositePath);
    }

    private static void X3i(CommandSyntax syntax)
    {
        CompositePath compositePath = UnpackPath(syntax.Arg);

        if (X3iCommand.CanExecute(compositePath))
            X3iCommand.Execute(compositePath);

    }

    private static void Mv(CommandSyntax syntax)
    {
        string[] args = syntax.Arg.Split("->");
        var source = UnpackPath(args[0]);
        var destination = UnpackPath(args[1]);

        if (MvCommand.CanExecuteSource(source) && MvCommand.CanExecuteDestination(destination))
        {
            var fileToCreate = UnpackPath($"{destination.FullPath}/{source.LastArgName}");
            MvCommand.Execute(fileToCreate, source);
        }
    }

    private static void Cp(CommandSyntax syntax)
    {
        string[] args = syntax.Arg.Split("->");
        var source = UnpackPath(args[0]);
        var destination = UnpackPath(args[1]);

        if (CpCommand.CanExecuteSource(source) && CpCommand.CanExecuteDestination(destination))
            CpCommand.Execute(source, destination);
    }

    #endregion

    //proviamo a fare l'editor direttamente su questo progetto, ma tieni da conto la logica per il secondo progetto (potrebbe tornare utile in altre circostanze)
    //private static void Edit(string arg)
    //{
    //    //Static resources are accessed as is.
    //    //If i update it in this project, i cannot view the updated version on other projects...
    //    if (!FileExists(arg))
    //    {
    //        Console.WriteLine($"no such file: {arg}");
    //        return;
    //    }
    //    //var mainModule = Process.GetCurrentProcess().MainModule;

    //    //string currentProject = mainModule.ModuleName;
    //    //string[] exePath = mainModule.FileName.Split('\\');

    //    //string[] projects = GetProjectsName(exePath, currentProject);

    //    //exePath[exePath.Length - 1] = $"{projects[1]}.exe";
    //    //int occurenceCount = 0;

    //    ////This for is required to contruct path to second project (CustomEditor)
    //    ////It could be avoided by having different project name (Visual Studio) and GitHub repo...
    //    //for (int i = 0; i < exePath.Length; i++)
    //    //{
    //    //    if (exePath[i].Equals(projects[0]))
    //    //        occurenceCount++;

    //    //    if (occurenceCount == 2)
    //    //    {
    //    //        exePath[i] = projects[1];
    //    //        break;
    //    //    }
    //    //}

    //    //CurrentDir dir = GetCurrentDir(Tree[Tree.Count- 1]);

    //    //Console.WriteLine(Dirs[0]);
    //    //Process process = new();
    //    ////If file path is incorrcet for some reason(s), return from the method letting know the user what went wrong.
    //    //try
    //    //{
    //    //    process.StartInfo = new()
    //    //    {
    //    //        FileName = string.Join('\\', exePath),
    //    //        UseShellExecute = true,
    //    //        CreateNoWindow = false,
    //    //        Arguments = string.Join(" ", new string[2] { arg, JsonSerializer.Serialize(Dirs[0]) })
    //    //    };
    //    //    Console.WriteLine(process.StartInfo.Arguments);
    //    //    //avoid user to write on current process while editing a file

    //    //    process.Start();

    //    //    process.WaitForExit();
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    Console.WriteLine(ex.ToString());
    //    //    process.Kill();
    //    //    return;
    //    //}
    //}

    /// <summary>
    /// Gets all projects in current solution, filtering hidden folders (begins with '.')
    /// </summary>
    /// <param name="path">Path to the executable. Must be passed as string[]</param>
    /// <param name="reference">Directory name contained inseide one of the path elements</param>
    /// <returns>Projects name inside folder specified by the @reference parameter</returns>
    /// Esample:
    /// path = "C:\Users\Utente\Documents\CustomCLI\CustomCLI\bin\Debug\net7.0";
    /// reference = "CustomCLI";
    /// return folders name inside "C:\Users\Utente\Documents\CustomCLI" path
    /// 
    //private static string[] GetProjectsName(string[] splittedPath, string reference)
    //{
    //    reference =  Path.GetFileNameWithoutExtension(reference);
    //    var adjustedPath = string.Empty;

    //    foreach (string s in splittedPath)
    //    {
    //        adjustedPath += $"{s}\\";
    //        if (s.Equals(reference))
    //            break;
    //    }

    //    return Directory.GetDirectories(adjustedPath).Select(Path.GetFileName).Where(s => !s.Contains('.')).ToArray();
    //}
}
