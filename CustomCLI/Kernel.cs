using CustomCLI.Commands;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace CustomCLI;

public class Kernel
{
    //TODO:
    //Try creating a file in a folder outside the current folder
    //eg:
    //touch test/file.txt
    //We need this for the move command
    public static List<CurrentDir> Dirs { get; set; } = new() { new CurrentDir { Name = string.Empty, Dept = -1 } };
    public static bool IsExit { get; set; } = false;
    public static int Dept { get; set; } = 0;
    public static List<string> Tree { get; set; } = new() { string.Empty };

    /// <summary>
    /// Executes command based on user input
    /// </summary>
    /// <param name="args"></param>
    public static void Execute(string[] args)
    {
        if (args[0].Equals(string.Empty))
            return;

        if (Enum.TryParse<CliCommands>(args[0], ignoreCase: true, out var command))
        {
            switch (command)
            {
                case CliCommands.Help:
                    Help();
                    break;
                case CliCommands.Cls:
                    Cls();
                    break;
                case CliCommands.Echo:
                    CheckSyntax(args, CliCommands.Echo, Echo);
                    break;
                case CliCommands.Cd:
                    CheckSyntax(args, CliCommands.Cd, Cd);
                    break;
                case CliCommands.Fd:
                    CheckSyntax(args, CliCommands.Fd, Fd);
                    break;
                case CliCommands.Touch:
                    CheckSyntax(args, CliCommands.Touch, Touch);
                    break;
                case CliCommands.Rm:
                    CheckSyntax(args, CliCommands.Rm, Rm);
                    break;
                case CliCommands.Mkdir:
                    CheckSyntax(args, CliCommands.Mkdir, MkDir);
                    break;
                case CliCommands.Rmdir:
                    CheckSyntax(args, CliCommands.Rmdir, Rmdir);
                    break;
                case CliCommands.Ls:
                    Ls();
                    break;
                case CliCommands.Edit:
                    CheckSyntax(args, CliCommands.Edit, Edit);
                    break;
                case CliCommands.Cat:
                    CheckSyntax(args, CliCommands.Cat, Cat);
                    break;
                case CliCommands.Mv:
                    CheckSyntax(args, CliCommands.Mv, Mv);
                    break;
                case CliCommands.Exit:
                    IsExit = true;
                    break;
                default:
                    break;
            }
            return;
        }
        Console.WriteLine($"No such command: {args[0]}");
    }

    private static void CheckSyntax(string[] args, CliCommands command, Action<string> commandExec)
    {
        args = args.Where(w => !string.IsNullOrEmpty(w)).ToArray();

        switch (args.Length)
        {
            case 1:
                Console.WriteLine($"{command.ToString().ToLower()} requires param");
                break;
            case 2:
                int quoteCount = args[1].Count(c => c.Equals('"'));
                if (quoteCount is 2 or 0)
                    commandExec.Invoke(args[1].Replace("\"", ""));
                else
                    Console.WriteLine("Missing char: \"");

                break;
            case > 2:
                int count = 2;
                for (int i = 2; i < args.Length; i++)
                {
                    count++;
                    args[1] += $" {args[i]}";
                    if (args[i][args[i].Length - 1].Equals('"'))
                        break;
                }

                bool isQuoted = args[1][0].Equals('"') && args[1][args[1].Length - 1].Equals('"');
                if (isQuoted && args.Length == count)
                    commandExec.Invoke(args[1].Replace("\"", ""));
                else if (isQuoted)
                    Console.WriteLine($"{command.ToString().ToLower()} arguments exedeed: {string.Join(' ', args[count..args.Length])}");
                else
                    Console.WriteLine($"{command.ToString().ToLower()} arguments exedeed: {string.Join(' ', args[2..args.Length])}");

                break;
        }
    }

    public static CurrentDir GetCurrentDir()
    {
        return Dirs.FirstOrDefault(d => d.Name.Equals(Tree[Tree.Count - 1]) && d.Dept == Dept - 1);
    }
    public static CurrentDir GetFolderByPosition(string folder, int dept)
    {
        return Dirs.FirstOrDefault(d => d.Name.Equals(folder) && d.Dept == dept);
    }
    public static CompositePath UnpackPath(string arg)
    {
        string[] args = arg.Split('/');
        int fileIndex = args.Length - 1;
        return new CompositePath()
        {
            ArgsNum = args.Length,
            LastArgIndex = fileIndex,
            LastArgName = args[fileIndex],
            Folders = string.Join('/', args.Take(fileIndex))
        };
    }
    public static VirtualFile GetVirtualFile(string file)
    {
        return GetCurrentDir().Files.FirstOrDefault(f => f.Name.Equals(file));
    }
    public static bool IsFolderEmpty(CurrentDir dir)
    {
        //Get current directory from which the command has been called
        return dir.Folders.Count == 0 && dir.Files.Count == 0;
    }
    public static bool FileExists(string name)
    {
        CurrentDir? dir = GetCurrentDir();
        return dir.Files.Where(file => file.Name.Equals(name)).Count() != 0;
    }
    public static bool FolderExists(string name)
    {
        CurrentDir? dir = GetCurrentDir();
        name = name.Replace("/", "");
        return dir.Folders.Where(f => f.Name.Equals(name)).Count() != 0;
    }

    #region Commands
    private static void Help()
    {
        Console.WriteLine("Command list:");
        foreach (CliCommands command in Enum.GetValues(typeof(CliCommands)))
            Console.Write($"{command.ToString().ToLower()}\t{command.ToDescriptionString()}\n");
        Console.WriteLine();
    }

    private static void Cls() => Console.Clear();

    private static void Echo(string arg)
    {
        if (EchoCommand.CanExecute(arg))
            EchoCommand.Execute(arg);
    }

    private static void Cd(string arg)
    {
        if(CdCommand.CanExecute(arg))
            CdCommand.Execute(arg);
    }

    private static void Fd(string arg)
    {
        if (FdCommand.CanExecute(arg))
            FdCommand.Execute(arg);
    }

    private static void Touch(string arg)
    {
        CompositePath compositePath = UnpackPath(arg);
        var splittedArg = compositePath.LastArgName.Split('.');

        if (compositePath.ArgsNum > 1)
        {
            if (!FolderExists(compositePath.Folders))
            {
                Console.WriteLine($"No such folder: {compositePath.Folders}");
                return;
            }
            Cd(compositePath.Folders);

            if (Enum.TryParse<FileExtension>(splittedArg[splittedArg.Length - 1], ignoreCase: true, out var extension2))
            {
                ConsoleColor color = 0;

                switch (extension2)
                {
                    case FileExtension.Txt:
                        color = (ConsoleColor)FileExtension.Txt;
                        break;
                    case FileExtension.Cs:
                        color = (ConsoleColor)FileExtension.Cs;
                        break;
                    case FileExtension.Zip:
                        color = (ConsoleColor)FileExtension.Zip;
                        break;
                    case FileExtension.Exe:
                        color = (ConsoleColor)FileExtension.Exe;
                        break;
                }

                Dirs[Dept].Files.Add(new VirtualFile
                {
                    Color = color,
                    Name = compositePath.LastArgName,
                    Content = string.Empty
                });
            }

            Fd(compositePath.LastArgIndex.ToString());
            return;
        }

        if (Enum.TryParse<FileExtension>(splittedArg[splittedArg.Length - 1], ignoreCase: true, out var extension))
        {
            ConsoleColor color = 0;

            switch (extension)
            {
                case FileExtension.Txt:
                    color = (ConsoleColor)FileExtension.Txt;
                    break;
                case FileExtension.Cs:
                    color = (ConsoleColor)FileExtension.Cs;
                    break;
                case FileExtension.Zip:
                    color = (ConsoleColor)FileExtension.Zip;
                    break;
                case FileExtension.Exe:
                    color = (ConsoleColor)FileExtension.Exe;
                    break;
            }

            Dirs[Dept].Files.Add(new VirtualFile
            {
                Color = color,
                Name = compositePath.LastArgName,
                Content = string.Empty
            });
        }
    }

    private static void Rm(string arg)
    {
        CompositePath compositePath = UnpackPath(arg);

        #region Why not checking path-to-file before calling Cd?
        //The Cd method contains the logic to automatically handle composite paths (like folder/file.txt) and simple paths (like file.txt)
        //This approch reduces the cognitive load and simplifies the code
        #endregion
        Cd(compositePath.Folders);//if folder or file does not exist, the method returns

        //the command will always try to delete the specified file
        if (RmCommand.CanExecute(compositePath.LastArgName))
            RmCommand.Execute(compositePath.LastArgName);

        Fd(compositePath.LastArgIndex.ToString());
    }

    private static void MkDir(string arg)
    {
        CompositePath compositePath = UnpackPath(arg);
        //we can use the GetCurrentDir method and change directories when needed
        if (compositePath.ArgsNum > 1)
        {
            //we are repeating the same logic for the Cd method!!!
            if(!FolderExists(compositePath.Folders))
            {
                Console.WriteLine($"No such folder: {compositePath.Folders}");
                return;
            }
            Cd(compositePath.Folders);

            if (FolderExists(compositePath.LastArgName))
            {
                Console.WriteLine($"Folder {compositePath.LastArgName} already exists");
                return;
            }
            Dirs[Dept].Folders.Add(new VirtualFolder
            {
                Color = ConsoleColor.Blue,
                Name = compositePath.LastArgName
            });

            Dirs.Add(new CurrentDir
            {
                Name = compositePath.LastArgName,
                Dept = compositePath.LastArgIndex
            });
            Fd(compositePath.LastArgIndex.ToString());
            return;
        }

        if (FolderExists(compositePath.LastArgName))
        {
            Console.WriteLine($"Folder {compositePath.LastArgName} already exists");
            return;
        }

        Dirs[Dept].Folders.Add(new VirtualFolder
        {
            Color = ConsoleColor.Blue,
            Name = compositePath.LastArgName
        });

        Dirs.Add(new CurrentDir
        {
            Name = compositePath.LastArgName,
            Dept = Dept
        });
    }

    private static void Rmdir(string arg)
    {
        CompositePath compositePath = UnpackPath(arg);
        //folder(s) before last element
        //need to factor better the code
        if (compositePath.ArgsNum > 1)
        {
            if (!FolderExists(compositePath.Folders))
            {
                Console.WriteLine($"No such folder: {compositePath.Folders}");
                return;
            }
            Cd(compositePath.Folders);

            if (!FolderExists(compositePath.LastArgName))
            {
                Console.WriteLine($"No such folder: {compositePath.LastArgName}");
                return;
            }

            if (!IsFolderEmpty(GetFolderByPosition(compositePath.LastArgName, compositePath.LastArgIndex)))
            {
                //TODO:
                //Generate script that deleted all files in folder if the user wants to
                Console.WriteLine($"\n{compositePath.LastArgName} is not empty.\nRemove elements in it first");
                return;
            }

            Dirs[Dept].Folders.RemoveAll(r => r.Name.Equals(compositePath.LastArgName));
            Fd(compositePath.LastArgIndex.ToString());
            return;
        }

        if (!FolderExists(compositePath.LastArgName))
        {
            Console.WriteLine($"No such folder: {compositePath.LastArgName}");
            return;
        }

        if (!IsFolderEmpty(GetFolderByPosition(compositePath.LastArgName, Dept)))
        {
            //TODO:
            //Generate script that deleted all files in folder if the user wants to
            Console.WriteLine($"\n{compositePath.LastArgName} is not empty.\nRemove elements in it first");
            return;
        }
        
        Dirs[Dept].Folders.RemoveAll(r => r.Name.Equals(compositePath.LastArgName));
    }

    private static void Ls()
    {
        CurrentDir? dir = GetCurrentDir();
        foreach (VirtualFolder folder in dir.Folders)
        {
            Console.ForegroundColor = folder.Color;
            if (folder.Name.Contains(' '))
                Console.Write($"\"{folder.Name}\" ");
            else
                Console.Write($"{folder.Name} ");
        }

        foreach (VirtualFile file in dir.Files)
        {
            Console.ForegroundColor = file.Color;
            if (file.Name.Contains(' '))
                Console.Write($"\"{file.Name}\" ");
            else
                Console.Write($"{file.Name} ");
        }
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine();
    }

    private static void Edit(string arg)
    {
        CompositePath compositePath = UnpackPath(arg);
        ConsoleKeyInfo cki;
        StringBuilder sb = new();

        if (compositePath.ArgsNum > 1)
        {
            if (!FolderExists(compositePath.Folders))
            {
                Console.WriteLine($"No such folder: {compositePath.Folders}");
                return;
            }
            Cd(compositePath.Folders);

            Console.WriteLine($"Editing {compositePath.LastArgName}");

            do
            {
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.Enter:
                        sb.Append('\n');
                        Console.WriteLine();
                        break;
                    case ConsoleKey.Spacebar:
                        sb.Append(' ');
                        break;
                    case ConsoleKey.Oem4: // ?
                        sb.Append('?');
                        break;
                    case ConsoleKey.Escape:
                        Console.WriteLine();
                        break;
                    case ConsoleKey.OemComma:
                        sb.Append(',');
                        break;
                    default:
                        sb.Append(cki.Key.ToString().ToLower());
                        break;
                }
            }
            while (cki.Key != ConsoleKey.Escape);

            CurrentDir dir2 = GetCurrentDir();
            dir2.Files.FirstOrDefault(f => f.Name.Equals(compositePath.LastArgName)).Content = sb.ToString();

            Console.WriteLine("Exiting editor");
            Fd(compositePath.LastArgIndex.ToString());
            return;
        }
        if (!FileExists(arg))
        {
            Console.WriteLine($"no such file: {arg}");
            return;
        }

        Console.WriteLine($"Editing {arg}");

        do
        {
            cki = Console.ReadKey();
            switch (cki.Key)
            {
                case ConsoleKey.Enter:
                    sb.Append('\n');
                    Console.WriteLine();
                    break;
                case ConsoleKey.Spacebar:
                    sb.Append(' ');
                    break;
                case ConsoleKey.Oem4: // ?
                    sb.Append('?');
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine();
                    break;
                case ConsoleKey.OemComma:
                    sb.Append(',');
                    break;
                default:
                    sb.Append(cki.Key.ToString().ToLower());
                    break;
            }
        }
        while (cki.Key != ConsoleKey.Escape);

        CurrentDir dir = GetCurrentDir();
        dir.Files.FirstOrDefault(f => f.Name.Equals(arg)).Content = sb.ToString();

        Console.WriteLine("Exiting editor");
    }

    private static void Cat(string arg)
    {
        CompositePath compositePath = UnpackPath(arg);
        if(compositePath.ArgsNum > 1)
        {
            if (!FolderExists(compositePath.Folders))
            {
                Console.WriteLine($"No such folder: {compositePath.Folders}");
                return;
            }
            Cd(compositePath.Folders);

            VirtualFile? file2 = GetVirtualFile(compositePath.LastArgName);
            Console.WriteLine(file2.Content);

            Fd(compositePath.LastArgIndex.ToString());
            return;
        }
        VirtualFile? file = GetVirtualFile(arg);
        Console.WriteLine(file.Content);
    }
    private static void Mv(string arg)
    {
        string[] args = arg.Split("->");
        if (!FileExists(args[0]))
            Console.WriteLine($"No such file: {args[0]}");
        else if (!FolderExists(args[1]))
            Console.WriteLine($"No such folder: {args[1]}");
        else
        {
            Rm(args[0]);
            Console.WriteLine($"{args[1]}{args[0]}");
            Touch($"{args[1]}{args[0]}");
            Console.WriteLine("Arguments correct");
        }
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
