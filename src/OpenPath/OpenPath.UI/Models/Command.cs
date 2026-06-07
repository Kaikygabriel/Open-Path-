namespace OpenPath.UI.Models;

public class Command
{
    public Command()
    {
        
    }
    public Command(string getFolder, string move, string copy, string remove,string getFile)
    {
        GetFolder = getFolder;
        Move = move;
        Copy = copy;
        Remove = remove;
        GetFile = getFile;
    }

    public string GetFolder { get; set; }
    public string Move { get; set; }
    public string Copy { get; set; }
    public string Remove { get; set; }
    public string GetFile { get; set; }
}