using System;

namespace OpenPath.UI.Models;

public static  class Archives
{
    public static string Profile { get;  } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    public static  string Documents { get; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)  , "Documents");
    public static  string Downloads { get; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) ,"Downloads");
    public static  string Pictures { get;  } = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
    public static string Favorites { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
}