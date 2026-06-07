using System;

namespace OpenPath.UI.Models;

public sealed class File
{
    public File()
    {
        
    }
    public File(string path, string name, string extension)
    {
        Path = path;
        Name = name;
        Extension = extension;
    }

    public string Path { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public int Lenght { get; set; }
    public DateTime LastWriteTime { get; set; }
}