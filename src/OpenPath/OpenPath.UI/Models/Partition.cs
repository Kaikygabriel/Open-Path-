using System.Text.Json.Serialization;
using OpenPath.UI.Enuns;

namespace OpenPath.UI.Models;

public sealed class Partition
{
    [JsonConstructor]
    public Partition()
    {
        
    }

    public Partition(string name, string driveType, string pathRootDirectory, double totalGb, double totalFreeGb,string driveFormat)
    {
        Name = name;
        DriveType = driveType;
        PathRootDirectory = pathRootDirectory;
        TotalGB = double.Parse(totalGb.ToString("F2"));
        TotalFreeGB = double.Parse(totalFreeGb.ToString("F2"));
        DriveFormat = driveFormat;
    }

    public string Name { get; set; }
    public string DriveType { get; set; }
    public string PathRootDirectory { get; set; }
    public double TotalGB { get; set; } 
    public double TotalFreeGB { get; set; }
    public string DriveFormat { get; set; }
}