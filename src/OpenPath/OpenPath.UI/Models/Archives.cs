
namespace OpenPath.UI.Models;

public class Archives
{
    public Archives()
    {
        
    }
    public Archives(string profile, string documents, string downloads, string pictures, string favorites)
    {
        Profile = profile;
        Documents = documents;
        Downloads = downloads;
        Pictures = pictures;
        Favorites = favorites;
    }

    public string Profile { get; set; }
    public string Documents { get; set; }
    public string Downloads { get; set; }
    public string Pictures { get; set; }
    public string Favorites { get; set; }

    
}