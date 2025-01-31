namespace CustomCLI.FileSystem;

public class VirtualFile : IFSO
{
    public string Name { get; set; }
    public string Path { get; set; }
    public ConsoleColor Color { get; set; }
    public string Content { get; set; }
    public FileExtensions Extension { get; set; }
}
