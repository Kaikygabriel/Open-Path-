using System.Text.Json.Serialization;

namespace OpenPath.UI.Models;

public class Command
{
    [JsonConstructor]
    public Command()
    {
        
    }
    public Command(string getFolder, string move, string copy, string remove,string getFile,string getPartitions)
    {
        GetFolder = getFolder;
        Move = move;
        Copy = copy;
        Remove = remove;
        GetFile = getFile;
        GetPartitions = getPartitions;
    }

    public string GetFolder { get; set; }
    public string Move { get; set; }
    public string Copy { get; set; }
    public string Remove { get; set; }
    public string GetFile { get; set; }
    public string GetPartitions { get; set; }
}