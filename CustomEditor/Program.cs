using CustomCLI;
using System.Diagnostics;
using System.Text;

Console.WriteLine("New process instantiated");

string arg = args[0];

Console.WriteLine(args[1]);

if (arg.GetType() != typeof(string) || arg == null)
{
    Console.WriteLine("""
        Could not find argument
        Please, call this exe from the CistomCli to make it work properly
        
        Press any key to continue
        """);
    Console.ReadKey();
    Process.GetCurrentProcess().Kill();
}

CurrentDir dir = Kernel.GetCurrentDir(Kernel.Tree[Kernel.Tree.Count - 1]);

//var dirs = Kernel.Dirs.FirstOrDefault(d => d.Equals(dir));
//Console.WriteLine(JsonSerializer.Serialize(Kernel.Dirs[0]));

StringBuilder sb = new();

Console.WriteLine(Kernel.Dirs.Count);
//if(Console.ReadKey())
//string ctt = Console.ReadLine();

//while (ctt != string.Empty)
//{
//    sb.Append(ctt);
//    ctt = Console.ReadLine();
//}
sb.Append("Console.ReadLine()");

dir.Files.FirstOrDefault(f => f.Name.Equals(arg)).Content = sb.ToString();

Console.WriteLine("Process finshed");
