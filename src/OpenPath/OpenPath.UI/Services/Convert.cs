using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using OpenPath.UI.Enuns;
using OpenPath.UI.Models;

namespace OpenPath.UI.Services;

public class ConvertApp
{
    public static Directory FromWindows(string data, string path)
    {
        var normalizedPath = path.Trim();
        var directoryName = System.IO.Path.GetFileName(normalizedPath.TrimEnd('\\', '/'));

        if (string.IsNullOrWhiteSpace(directoryName))
            directoryName = normalizedPath;
        var directoryCreate = new Directory
        {
            Path = normalizedPath,
            Name = directoryName
        };

        var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var trimmed = line.TrimStart();

            // Ignora cabeçalho e linha separadora
            if (!(trimmed.StartsWith("d") || trimmed.StartsWith("-")))
                continue;

            // Separa por 2 ou mais espaços, que é como a tabela está formatada
            var parts = Regex.Split(trimmed, @"\s{2,}")
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToArray();

            // Exemplo:
            // d-----        01/06/2026     14:09                .idea
            // parts[0] = d-----
            // parts[1] = 01/06/2026
            // parts[2] = 14:09
            // parts[3] = .idea

            if (parts.Length < 4)
                continue;
            if (!DateTime.TryParse($"{parts[1]} {parts[2]}", out var date))
                date = DateTime.MinValue;
 
            var mode = parts[0];
            var itemName = parts[3].Trim();
            System.IO.File.AppendAllText(
                @"debug.txt",
                $" \t \n {string.Join(',',parts)} \t \t ");

            if (string.IsNullOrWhiteSpace(itemName))
                continue;

            var itemPath = System.IO.Path.Combine(normalizedPath, itemName);

            if (parts[0].StartsWith("d"))
            {
                var lengthRaw = parts.Length > 4 ? parts[3] : "0";
                int.TryParse(lengthRaw, out var length);
                var directoryNew = new Directory
                {
                    Date = date,
                    Name = itemName,
                    Path = itemPath
                };

                directoryCreate.Directories.Add(directoryNew);
            }

            if (mode.StartsWith("-", StringComparison.OrdinalIgnoreCase))
            {
                var fileNew = new File
                {
                    LastWriteTime = date,
                    Name = itemName.Split(' ').LastOrDefault() ?? itemName,
                    Path = System.IO.Path.Combine(normalizedPath, itemName.Split(' ').LastOrDefault() ?? itemName),
                    Extension = System.IO.Path.GetExtension(itemName.Split(' ').LastOrDefault() ?? itemName)
                };
                if (int.TryParse(itemName.Split(' ').First(), out int lenght))
                {
                    fileNew.Lenght = lenght;
                }
                directoryCreate.Files.Add(fileNew);
            }
        }

        return directoryCreate;
    }

    public static List<Partition> GetPartitionsFromWindows(string data)
    {
        // System.IO.File.AppendAllText(
        //     @"debug.txt",
        //     $" \t \n  DATAAAAAAAAA :::::::: {data} \t \t ");

        var teste = JsonSerializer.Deserialize<List<Partition>>(data);
        foreach(var p in teste)
        {
            System.IO.File.AppendAllText(
                @"debug.txt",
                $" \t \n {JsonSerializer.Serialize(p)} \t \t ");
            p.Type = GetDriveType(p.DriveType);
        }
        
        return teste;
        // var list = new List<Partition>();
        // var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        //
        //
        // System.IO.File.AppendAllText(
        //     @"debug.txt",
        //     $" \t \n  LINES :  {lines.Length} \t \t ");
        //
        // foreach (var line in lines)
        // {
        //     var trimmed = line.TrimStart();
        //
        //     // Ignora cabeçalho e linha separadora
        //     if (trimmed.Equals("-----------") || trimmed.StartsWith("DriveLetter"))
        //         continue;
        //
        //     // Separa por 2 ou mais espaços, que é como a tabela está formatada
        //     var parts = Regex.Split(trimmed, @"\s{2,}")
        //         .Where(part => !string.IsNullOrWhiteSpace(part))
        //         .ToArray();
        //
        //     System.IO.File.AppendAllText(
        //         @"debug.txt",
        //         $" \t \n  LINES  OF LINE:  {parts.Length} -- {string.Join(',', parts)} \t \t ");
        // }
        //
        // foreach (var line in lines)
        // {
        //     var trimmed = line.TrimStart();
        //
        //     // Ignora cabeçalho e linha separadora
        //     if (trimmed.Equals("-----------") || trimmed.StartsWith("DriveLetter"))
        //         continue;
        //
        //     // Separa por 2 ou mais espaços, que é como a tabela está formatada
        //     var parts = Regex.Split(trimmed, @"\s{2,}")
        //         .Where(part => !string.IsNullOrWhiteSpace(part))
        //         .ToList();
        //
        //     System.IO.File.AppendAllText(
        //         @"debug.txt",
        //         $" \t \n  LINES  OF LINE:  {parts.Count} -- {string.Join(',',parts)} \t \t ");
        //
        //     if (parts.Count < 5 && parts.Count != 4)
        //         continue;
        //
        //     var name = "";
        //     var systemType = "";
        //     if (parts.Count == 4 )
        //     {
        //         var listNameAndFileSystem = parts[1].Split(' ');
        //         var last = listNameAndFileSystem.Last();
        //         name = string.Join(' ', listNameAndFileSystem.Where(x=>x != last).ToList());
        //         systemType = last;
        //     }
        //     else
        //     {
        //         name = parts[1];
        //         systemType = parts[2];
        //     }
        //     
        //     var driverLetter  = parts[0];
        //     var driveType = parts[3];
        //     var tryGetSizeFree = float.TryParse(parts[6],out float sizeFree);
        //     var tryGetSizeTotal = float.TryParse(parts[7],out float sizeTotal);
        //     
        //     if (string.IsNullOrEmpty(driverLetter) ||
        //         string.IsNullOrEmpty(systemType) ||
        //         string.IsNullOrEmpty(driveType) ||
        //         !tryGetSizeFree ||
        //         !tryGetSizeTotal )
        //         continue;
        //     
        //     list.Add(new (driverLetter + "/",name,systemType,sizeFree,sizeTotal,GetDriveType(driveType)));
        // }
        //
        // return list;
    }

    private static EDriveType GetDriveType(string type)
    {
        if (type.Equals("Fixed"))
            return EDriveType.Fixed;
        
        return EDriveType.Removable;
    }
}