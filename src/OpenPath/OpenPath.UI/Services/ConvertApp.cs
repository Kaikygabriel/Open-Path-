using System.Collections.Generic;
using System.Text.Json;
using OpenPath.UI.Enuns;
using OpenPath.UI.Models;

namespace OpenPath.UI.Services;

public class ConvertApp
{
    public static List<Partition> GetPartitionsFromWindows(string data)
    {
        var partitions = JsonSerializer.Deserialize<List<Partition>>(data);
        foreach(var p in partitions!)
        {
            p.Type = GetDriveType(p.DriveType);
        }
        
        return partitions;
    }

    private static EDriveType GetDriveType(string type)
    {
        if (type.Equals("Fixed"))
            return EDriveType.Fixed;
        
        return EDriveType.Removable;
    }
}