namespace CustomCLI;

public class CommandSyntax
{
    public string? Option { get; set; }
    public string? Arg { get; set; }

    /// <summary>
    /// Removes all empty strings inside the given string[]
    /// </summary>
    /// <param name="args">Argument to remove empty strings from</param>
    /// <returns>A new string[] without empty strings</returns>
    public static string[] TrimArgs(string[] args) =>
        args.Where(arg => !string.IsNullOrEmpty(arg)).ToArray();
}
