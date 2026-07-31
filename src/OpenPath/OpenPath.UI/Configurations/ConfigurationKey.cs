using System;

namespace OpenPath.UI.Configurations;

public static class ConfigurationKey
{
    public static string PathConfigurationMode { get;}= System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)  , "Documents","OpenPath","ConfigurationMode.txt"); 
}