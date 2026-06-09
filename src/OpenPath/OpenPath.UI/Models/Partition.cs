using System.Text.Json.Serialization;
using OpenPath.UI.Enuns;

namespace OpenPath.UI.Models;

public sealed class Partition
{
    [JsonConstructor]
    public Partition()
    {
        
    }
    
    public Partition(string driveLetter, string fileSystemLabel, string fileSystemType, float sizeGb, float sizeRemainingGb, string driveType, EDriveType type)
    {
        DriveLetter = driveLetter;
        FileSystemLabel = fileSystemLabel;
        FileSystemType = fileSystemType;
        SizeGB = sizeGb;
        SizeRemainingGB = sizeRemainingGb;
        DriveType = driveType;
        Type = type;
    }

    public string DriveLetter { get; set; }
    public string FileSystemLabel { get; set; }
    public string FileSystemType { get; set; }

    public float SizeGB { get; set; } = 0;
    public float SizeRemainingGB { get; set; }
    public string DriveType { get; set; }
    
    public EDriveType Type { get; set; }
}