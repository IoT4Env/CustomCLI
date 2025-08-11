namespace CustomCLI;

public class CurrentDir
{
    private string name = string.Empty;
    public string Name { 
        get => name;
        set => name = value;
    }
    private int dept = 0;
    public int Dept
    {
        get => dept;
        set => dept = value;
    }
    public List<VirtualFolder> Folders { get; set; } = new();
    public List<VirtualFile> Files { get; set; } = new();
}
