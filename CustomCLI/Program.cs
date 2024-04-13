using CustomCLI;

Console.WriteLine("""
    CLI tools from scratch by BioTech4Env
    Made to refine C# skills

    """);

while (!Kernel.IsExit)
{
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write(@$"Z:{string.Join("\\", Kernel.Tree)}>");
    string[] userInput = Console.ReadLine().Split(' ');

    Kernel.Execute(userInput);
}

Console.WriteLine("Exiting CLI");
