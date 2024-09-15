namespace CustomCLI;

public class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
{
    public string Name { get; protected init; }
    public string Description { get; protected init; }
    public bool HasArg { get; protected init; }
    public Action<CommandSyntax> CheckSyntax { get; protected init; }

    public Enumeration(string name, string description, bool hasArg, Action<CommandSyntax> syntax)
    {
        Name = name;
        Description = description;
        HasArg = hasArg;
        CheckSyntax = syntax;
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }
        return GetType() == other.GetType();
    }
}
