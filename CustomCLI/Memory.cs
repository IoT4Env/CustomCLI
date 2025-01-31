namespace CustomCLI;

public static class Memory
{
    /// <summary>
    /// The heap memory.
    /// Data allocated in the Heap is refered by a string literal the same way a variable stores its content
    /// </summary>
    public static Dictionary<string, object> Heap { get; set; } = new();
    public static Stack<string> StackSegment { get; set; } = new();

    public static bool CanResolveVariables(string[] args)
    {
        return true;
    }

    public static string[] ResolveVariables(string[] args)
    {
        return new string[0];
    }
}
