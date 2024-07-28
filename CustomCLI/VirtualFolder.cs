﻿namespace CustomCLI;

public class VirtualFolder
{
    public int Dept { get; set; }
    public string Name { get; set; }
    public ConsoleColor Color { get; set; }
    public List<VirtualFolder> Folders { get; set; } = new();
    public List<VirtualFile> Files { get; set; } = new();
}
