using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO;

namespace OpenPath.UI.Models;

public class ManagerCommands
{
    public ManagerCommands()
    {
        
    }
    

    public Directory GetFiles(string path)
    {   
        path = path.Trim();
        
        path = path.Replace("\r", "")
            .Replace("\n", "");
    
        var directoryInfo = new DirectoryInfo(path);
        var directory = new Directory()
        {
            Date = directoryInfo.CreationTime,
            Name = directoryInfo.Name,
            Path = directoryInfo.FullName,
        };
        foreach(var item in directoryInfo.GetDirectories())
        {
            directory.Directories.Add(new Directory()
            {
                Name = item.Name,
                Path = item.FullName,
                Date = item.LastWriteTime
            });
        }

        foreach (var item in directoryInfo.GetFiles())
        {
            directory.Files.Add(new File()
            {
                Name = item.Name,
                Path = item.FullName,
                LastWriteTime = item.LastWriteTime,
                Lenght = File.FormatBytes(item.Length)
            });
        }
        return directory;
    }

        public List<Partition> GetPartition()
        {
            var drives = DriveInfo.GetDrives();
            var listDrives = new List<Partition>();
            foreach (var drive in drives)
            {
                try
                {

                    if(drive.IsReady)
                        listDrives.Add(new Partition(
                            drive.Name,
                            drive.DriveType.ToString(),
                            drive.RootDirectory.FullName,
                            drive.TotalSize / 1024.0 / 1024.0 / 1024.0,
                            drive.AvailableFreeSpace / 1024.0 / 1024.0 / 1024.0,
                            drive.DriveFormat));
                }
                catch (Exception e)
                {
                    continue;
                }
            }
           
            return listDrives;
        }
        
    public void OpenFile(string path, string? program = null)
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = path,
            UseShellExecute = true
        };

        Process.Start(startInfo);
    }

    public async Task CreateFolder(string path, string name)
    {
        var fullDirectoryName = Path.Combine(path, name);
        System.IO.Directory.CreateDirectory(fullDirectoryName);
    }
    
    public async Task CreateFile(string path, string name)
    {
        var fullNameFile = Path.Combine(path, name);
        System.IO.File.Create(fullNameFile);
    }
} 