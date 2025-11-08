using CustomCLI;


//Sets OS specific variables to be accessed globally throughout the CLI
HostOS.Instantiate();

//Configure terminal text colors
Console.ForegroundColor = ConsoleColor.Gray;

//Welcomeing text
Console.WriteLine("""
    CLI tools from scratch by IoT4Env
    Made to refine C# skills
    Type "help" (without quotation marks) to get an overview of the current working commands

    """);

while (!Kernel.IsExit)
{
    Console.Write(@$"Z:{string.Join("/", Kernel.Tree)}>");
    string[] userInput = Console.ReadLine().Split(' ');

    Kernel.Execute(userInput);
}

Console.WriteLine("Exiting CLI");
