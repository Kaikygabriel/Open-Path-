using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using OpenPath.UI.Models;
using OpenPath.UI.Services;

namespace OpenPath.UI.ViewModels;

public class ExplorerViewModel : INotifyPropertyChanged
{
    private Directory _currentDirectory = new()
    {
        Name = "Carregando..."
    };

    private List<Partition> _partitions = [];
    
    public List<Partition> Partitions
    {
        get => _partitions;
        set
        {
            _partitions = value;
            OnPropertyChanged();
        }
    }
    
    public string Next { get; set; }= "";
    public string Previous { get; set; } = "";
    public Directory CurrentDirectory
    {
        get => _currentDirectory;
        set
        {
            if (!string.IsNullOrEmpty(_currentDirectory.Path))
            {
                if (_currentDirectory.Path.Split('/').Length > value.Path.Split('/').Length ||
                    _currentDirectory.Path.Split('\\').Length > value.Path.Split('\\').Length)
                {
                    Next = _currentDirectory.Path;
                }
            }
            
            _currentDirectory = value;
            
            if (OperatingSystem.IsWindows())
            {
                var teste  = _currentDirectory.Path.Split('\\').ToList();
                teste.Remove(teste.Last());
                
                if (teste.Count  >= 0)
                {
                    Previous = string.Join('\\',teste) ?? "";
                }
                else
                {
                    Previous = "";
                }
            }
            else
            {
                var teste  = _currentDirectory.Path.Split('/').ToList();
                teste.Remove(teste.Last());
                
                if (teste.Count  >= 0)
                {
                    Previous = string.Join('/',teste) ?? "";
                }
                else
                {
                    Previous = "";
                }
            } 

            
            OnPropertyChanged();
        }
    }

    public ExplorerViewModel()
    {
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            var manager = new ManagerCommands(
                @"Assets/SystemCommandsWindows.json");

            var path = await manager.GetArchive(manager.Archives.Profile);
            
            CurrentDirectory = await manager.GetFiles(path);

            Partitions = ConvertApp.GetPartitionsFromWindows(await manager.GetPartition());
            

        }
        catch(Exception e )
        {
                   
            await System.IO.File.AppendAllTextAsync(
                @"debug.txt",
                $" \t \n  exceção {e .Message} \t \t ");
            
            CurrentDirectory = new Directory
            {
                Name = "Erro ao carregar"
            };
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    
}