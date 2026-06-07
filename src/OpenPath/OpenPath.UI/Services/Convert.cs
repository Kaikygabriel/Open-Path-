using System;
using System.Linq;
using System.Text.RegularExpressions;
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
}