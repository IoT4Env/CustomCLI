namespace CustomCLI.FileSystem;

public class VirtualFolder : IFSO
{
    public int Dept { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public ConsoleColor Color { get; set; }
    public List<VirtualFolder> Folders { get; set; } = new();
    public List<VirtualFile> Files { get; set; } = new();
}
