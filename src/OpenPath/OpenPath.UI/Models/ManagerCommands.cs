using System;
using System.Text.Json;
using OpenPath.UI.Configurations;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OpenPath.UI.Services;

namespace OpenPath.UI.Models;

public class ManagerCommands
{
    [JsonConstructor]
    public ManagerCommands()
    {
        
    }
    public ManagerCommands(string path)
    {
        ManagerCommands? manager = null;

        if (OperatingSystem.IsWindows())
        {
            var json = System.IO.File.ReadAllText(path); 

            manager =  JsonSerializer.Deserialize<ManagerCommands>(json);
            Tool = "powershell.exe";
        }

        if (manager is null)
        {
        //     System.IO.File.AppendAllText(
        //         @"debug.txt",
        //         "\n Deu Exceção de null");
        //
        //     throw new NullReferenceException("Archives Commands Not Found");
        }

        Commands = manager.Commands;
        Archives = manager.Archives;
    }

    public Command Commands { get; set; }
    public Archives Archives { get; set; }
    public string Tool { get; set; }

    public async Task<Directory> GetFiles(string path)
    {
        var process = new Process();
        
        process.StartInfo.FileName = Tool;
        process.StartInfo.Arguments = $"-Command \"{Commands.GetFolder}\" {path} ";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();
        
        var response = ConvertApp.FromWindows(output,path);
        
        return response;
    }
    public async Task<string> GetArchive(string commandPath)
    {
        var process = new Process();
        
        process.StartInfo.FileName = Tool;
        process.StartInfo.Arguments = $"-Command {commandPath}";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        return output;
    }

    public async Task<string> GetPartition()
    {
        var process = new Process();
        
        process.StartInfo.FileName = Tool;
        process.StartInfo.Arguments = $"-Command {Commands.GetPartitions}";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        if (process.ExitCode != 0)
        {
            System.IO.File.AppendAllText(
                @"debug.txt",
                $"\n         ExitCode: {process.ExitCode}\n        Output: {output}\n        Error: {error}\n        ");
            // Faça algo com a string 'error' aqui (ex: log ou lançar exception)
        }  
        
        return output;
    }
    
    public async Task OpenFile(string path, string? program = null)
    {
        System.IO.File.AppendAllText(
                 @"debug.txt",
                 $"\n  ESSE EO PATH A INICIAR{path}");
        using var process = new Process();
    
        process.StartInfo.FileName = Tool;
        process.StartInfo.Arguments = $"""-Command start '{path}' """;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
    
        process.Start();

        // 2. Dispara a leitura de ambos os fluxos simultaneamente para evitar deadlocks
        Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
        Task<string> errorTask = process.StandardError.ReadToEndAsync();

        // 3. Aguarda a leitura completa e o término do processo de forma assíncrona
        await Task.WhenAll(outputTask, errorTask, process.WaitForExitAsync());

        string output = outputTask.Result;
        string error = errorTask.Result;

        // Opcional: Tratar o erro se o processo falhar (ExitCode diferente de 0)
        if (process.ExitCode != 0)
        {
            System.IO.File.AppendAllText(
                    @"debug.txt",
                    $"\n         ExitCode: {process.ExitCode}\n        Output: {output}\n        Error: {error}\n        ");
            // Faça algo com a string 'error' aqui (ex: log ou lançar exception)
        }    
        
    }
} 