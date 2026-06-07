using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenPath.UI.Models;

public class Directory
{
    public string Path { get; set; }
    public List<Directory> Directories { get; set; } = [];
    public List<File> Files { get; set; } = [];
    public DateTime Date { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return
            $"{Path} , {Name} , {Date} ... DIRECTORIES : \n {string.Join(',', Directories.Select(x=>x.Name))}\n FILES :{string.Join(',', Files.Select(x => x.Name))} ";
    }
}