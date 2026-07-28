using System;
using Avalonia.Media.Imaging;

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
    public string Lenght { get; set; }
    public DateTime LastWriteTime { get; set; }
    public Bitmap Icon { get; set; }
    public static string FormatBytes(long bytes)
    {
        string[] suffixes = ["B", "KB", "MB", "GB", "TB"];
        double size = bytes;
        int suffix = 0;

        while (size >= 1024 && suffix < suffixes.Length - 1)
        {
            size /= 1024;
            suffix++;
        }

        return $"{size:0.##} {suffixes[suffix]}";
    }
}