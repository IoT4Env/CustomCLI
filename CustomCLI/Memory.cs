namespace CustomCLI;

//Try to make this class heredit from a ICommand class to use for variables resolution
public static class Memory
{
    private static char VariablePrefix { get; set; } = '$';
    /// <summary>
    /// The heap memory.
    /// Data allocated in the Heap is refered by a string literal the same way a variable stores its content
    /// </summary>
    public static Dictionary<string, string> Heap { get; set; } = new();
    public static Stack<string> StackSegment { get; set; } = new();

    /// <summary>
    /// Checks if variables used inside the command are defined in the Heap
    /// </summary>
    /// <param name="args">Command arguments</param>
    /// <param name="undefined">The not defined variable name</param>
    /// <returns>true if passed variables (if any) are found in the Heap</returns>
    public static bool CanResolveVariables(string[] args, out string undefined)
    {
        undefined = string.Empty;
        foreach(string arg in args)
        {
            if(arg[0] == VariablePrefix && !Heap.TryGetValue(arg, out var value))
            {
                undefined = arg[1..arg.Length];
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Maps variable names with the corresponding value
    /// </summary>
    /// <param name="args">Command arguments</param>
    /// <returns>A list of object with variable names substituted with their value</returns>
    public static string[] ResolveVariables(string[] args)
    {
        List<string> resolvedArgs = new();
        foreach (string arg in args)
        {
            if(Heap.TryGetValue(arg, out var value))
            {
                resolvedArgs.Add(value);
                continue;
            }
            resolvedArgs.Add(arg);
        }
        return resolvedArgs.ToArray();
    }
}
