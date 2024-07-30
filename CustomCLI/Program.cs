using CustomCLI;

Console.WriteLine("""
    CLI tools from scratch by IoT4Env
    Made to refine C# skills
    Type "help" (without quotation marks) to get an overview of the current working commands

    """);

while (!Kernel.IsExit)
{
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write(@$"Z:{string.Join("/", Kernel.Tree)}>");
    string[] userInput = Console.ReadLine().Split(' ');

    Kernel.Execute(userInput);
}

Console.WriteLine("Exiting CLI");
