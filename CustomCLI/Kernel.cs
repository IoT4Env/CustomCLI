using System.Diagnostics;
using System.Reflection;

namespace CustomCLI;

public static class Kernel
{
    public static List<CurrentDir> Dirs { get; private set; } = new() { new CurrentDir { Name = string.Empty, Dept = -1 } };
    public static bool IsExit { get; private set; } = false;
    public static int Dept { get; private set; } = 0;
    public static List<string> Tree { get; private set; } = new() { string.Empty };

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

    private static bool IsFolderEmpty(string name)
    {
        CurrentDir dir = Dirs.FirstOrDefault(f => f.Name.Equals(name) && f.Dept == Dept);
        return dir.Folders.Count == 0 && dir.Files.Count == 0;
    }

    private static void Help()
    {
        Console.WriteLine("Command list:");
        foreach (CliCommands command in Enum.GetValues(typeof(CliCommands)))
            Console.Write($"{command.ToString().ToLower()}\t{command.ToDescriptionString()}\n");
        Console.WriteLine();
    }

    private static void Cls() => Console.Clear();

    private static void Echo(string arg) => Console.WriteLine(arg);

    private static void Cd(string arg)
    {
        if (Dirs[Dept].Folders.Select(s => s.Name)
            .Where(w => w.Equals(arg))
            .Count() == 0)
        {
            Console.WriteLine($"No such directory: {arg}");
            return;
        }

        Dept++;
        Tree.Add(arg);
    }

    private static void Fd(string dept)
    {
        int deptInt = Convert.ToInt32(dept);
        List<string> dirTree = Tree.Where(w => !string.IsNullOrEmpty(w)).ToList();
        if (dirTree.Count < deptInt)
        {
            Console.WriteLine($"Dept too large for current tree: {Tree}");
            return;
        }
        Tree.RemoveAt(Tree.Count - 1);
        Dept--;
    }

    private static void Touch(string arg)
    {
        var splittedArg = arg.Split('.');
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
                Name = arg
            });
        }
    }

    private static void Rm(string arg)
    {
        if (Dirs[Dept].Files.Select(s => s.Name).FirstOrDefault(f => f.Equals(arg)) == null)
        {
            Console.WriteLine($"No such file: {arg}");
            return;
        }
        Dirs[Dept].Files.RemoveAll(r => r.Name.Equals(arg));
    }

    private static void MkDir(string arg)
    {
        if (Dirs[Dept].Folders.Select(s => s.Name).FirstOrDefault(f => f.Equals(arg)) != null)
        {
            Console.WriteLine($"Folder {arg} already exists");
            return;
        }

        Dirs[Dept].Folders.Add(new VirtualFolder
        {
            Color = ConsoleColor.Blue,
            Name = arg
        });

        Dirs.Add(new CurrentDir
        {
            Name = arg,
            Dept = Dept
        });
    }

    private static void Rmdir(string arg)
    {
        if (Dirs[Dept].Folders.Select(s => s.Name).FirstOrDefault(f => f.Equals(arg)) == null)
        {
            Console.WriteLine($"No such folder: {arg}");
            return;
        }

        if (!IsFolderEmpty(arg))
        {
            Console.WriteLine($"\n{arg} is not empty.\nRemove elements in it first");
            return;
        }
        Dirs[Dept].Folders.RemoveAll(r => r.Name.Equals(arg));
    }

    private static void Ls()
    {
        CurrentDir dir = Dirs.FirstOrDefault(f => f.Name.Equals(Tree[Tree.Count - 1]) && f.Dept == Dept - 1);
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
        var mainModule = Process.GetCurrentProcess().MainModule;

        var exePath = mainModule.FileName.Split('\\');
        var exeName = Path.GetFileNameWithoutExtension(mainModule.ModuleName);
        var adjustedPath = string.Empty;

        foreach(string s in exePath)
        {
            adjustedPath += $"{s}\\";
            if (s.Equals(exeName))
                break;
        }

        string[] projects = Directory.GetDirectories(adjustedPath).Select(Path.GetFileName).Where(s => !s.Contains('.')).ToArray();

        var cePathSplitted = Path.GetFullPath($"{projects[1]}.exe").Split('\\');
        var occurenceCount = 0;

        for (int i = 0; i < cePathSplitted.Length; i++)
        {
            if (cePathSplitted[i].Equals(projects[0]))
                occurenceCount++;

            if (occurenceCount == 2)
            {
                cePathSplitted[i] = projects[1];
                break;
            }
        }

        string cePathAdjusted = string.Join('\\', cePathSplitted);

        //If file path is incorrcet for some reason(s), return from the method letting know the user what went wrong.
        try
        {
            Process process = new();
            process.StartInfo.FileName = cePathAdjusted;

            process.Start();

            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }
    }
}
