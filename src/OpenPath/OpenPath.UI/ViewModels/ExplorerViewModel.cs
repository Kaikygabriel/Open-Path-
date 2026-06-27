using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using OpenPath.UI.Models;
using OpenPath.UI.Services;
using System.Text.Json;

namespace OpenPath.UI.ViewModels;

public class ExplorerViewModel : INotifyPropertyChanged
{
    private Directory _currentDirectory = new()
    {
        Name = "Carregando..."
    };

    private List<Partition> _partitions = [];

    private Directory? _selectedDirectory;
    private File? _selectedFile;

    public List<Partition> Partitions
    {
        get => _partitions;
        set
        {
            _partitions = value;
            OnPropertyChanged();
        }
    }
    
    private bool _hasVisibleCreateFile;

    public bool HasVisibleCreateFile
    {
        get => _hasVisibleCreateFile;
        set
        {
            if (_hasVisibleCreateFile == value)
                return;

            _hasVisibleCreateFile = value;
            OnPropertyChanged();
        }
    }
    
    private bool _hasVisibleCreateFolder;

    public bool HasVisibleCreateFolder
    {
        get => _hasVisibleCreateFolder;
        set
        {
            if (_hasVisibleCreateFolder == value)
                return;

            _hasVisibleCreateFolder = value;
            OnPropertyChanged();
        }
    }
    
    public string FileName { get; set; }
    public string FolderName { get; set; }
    
    public string Next { get; set; } = "";
    public string Previous { get; set; } = "";

    public Directory CurrentDirectory
    {
        get => _currentDirectory;
        set
        {
            if (!string.IsNullOrEmpty(_currentDirectory.Path) && !string.IsNullOrEmpty(value.Path))
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
                var teste = _currentDirectory.Path.Split('\\').ToList();
                if (teste.Count > 0)
                    teste.Remove(teste.Last());

                Previous = teste.Count > 0 ? string.Join('\\', teste) : "";
            }
            else
            {
                var teste = _currentDirectory.Path.Split('/').ToList();
                if (teste.Count > 0)
                    teste.Remove(teste.Last());

                Previous = teste.Count > 0 ? string.Join('/', teste) : "";
            }

            SelectedDirectory = null;
            SelectedFile = null;

            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedDirectory));
            OnPropertyChanged(nameof(SelectedFile));
            OnPropertyChanged(nameof(HasSelection));
            OnPropertyChanged(nameof(SelectedName));
            OnPropertyChanged(nameof(SelectedKind));
            OnPropertyChanged(nameof(SelectedPath));
            OnPropertyChanged(nameof(SelectedIcon));
            OnPropertyChanged(nameof(SelectedPrimaryInfo));
            OnPropertyChanged(nameof(SelectedModifiedText));
        }
    }

    public Directory? SelectedDirectory
    {
        get => _selectedDirectory;
        set
        {
            if (Equals(_selectedDirectory, value))
                return;

            _selectedDirectory = value;
            if (value is not null)
                _selectedFile = null;

            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedFile));
            OnPropertyChanged(nameof(HasSelection));
            OnPropertyChanged(nameof(SelectedName));
            OnPropertyChanged(nameof(SelectedKind));
            OnPropertyChanged(nameof(SelectedPath));
            OnPropertyChanged(nameof(SelectedIcon));
            OnPropertyChanged(nameof(SelectedPrimaryInfo));
            OnPropertyChanged(nameof(SelectedModifiedText));
        }
    }

    public File? SelectedFile
    {
        get => _selectedFile;
        set
        {
            if (Equals(_selectedFile, value))
                return;

            _selectedFile = value;
            if (value is not null)
                _selectedDirectory = null;

            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedDirectory));
            OnPropertyChanged(nameof(HasSelection));
            OnPropertyChanged(nameof(SelectedName));
            OnPropertyChanged(nameof(SelectedKind));
            OnPropertyChanged(nameof(SelectedPath));
            OnPropertyChanged(nameof(SelectedIcon));
            OnPropertyChanged(nameof(SelectedPrimaryInfo));
            OnPropertyChanged(nameof(SelectedModifiedText));
        }
    }

    public bool HasSelection => SelectedDirectory is not null || SelectedFile is not null;

    public string SelectedName =>
        SelectedDirectory?.Name ??
        SelectedFile?.Name ??
        string.Empty;

    public string SelectedKind =>
        SelectedDirectory is not null ? "Pasta" :
        SelectedFile is not null ? "Arquivo" :
        string.Empty;

    public string SelectedPath =>
        SelectedDirectory?.Path ??
        SelectedFile?.Path ??
        string.Empty;

    public string SelectedIcon =>
        SelectedDirectory is not null ? "/Assets/directory.png" :
        SelectedFile is not null ? "/Assets/file.png" :
        string.Empty;

    public string SelectedPrimaryInfo =>
        SelectedDirectory is not null
            ? $"{SelectedDirectory.Directories.Count} subpastas • {SelectedDirectory.Files.Count} arquivos"
            : SelectedFile is not null
                ? $"Tamanho: {FormatBytes(SelectedFile.Lenght)}"
                : string.Empty;

    public string SelectedModifiedText =>
        SelectedDirectory is not null
            ? $"Atualizado em: {SelectedDirectory.Date:dd/MM/yyyy HH:mm}"
            : SelectedFile is not null
                ? $"Modificado em: {SelectedFile.LastWriteTime:dd/MM/yyyy HH:mm}"
                : string.Empty;

    public ExplorerViewModel()
    {
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            await System.IO.File.AppendAllTextAsync(
                @"debug.txt",
                $" \t \n  EXECUTOU \t \t ");
            var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");

            var path = await manager.GetArchive(manager.Archives.Profile);

            CurrentDirectory = await manager.GetFiles(path);
            await System.IO.File.AppendAllTextAsync(
                @"debug.txt",
                $" \t \n  {path} \t \t ");

            Partitions = ConvertApp.GetPartitionsFromWindows(await manager.GetPartition());
        }
        catch (Exception e)
        {
            await System.IO.File.AppendAllTextAsync(
                @"debug.txt",
                $" \t \n  exceção {e.Message} {e.StackTrace} \t \t ");
            
            await System.IO.File.AppendAllTextAsync(
                @"debug.txt",
                $" \t \n  exceção {JsonSerializer.Serialize(e)} \t \t ");

            CurrentDirectory = new Directory
            {
                Name = "Erro ao carregar"
            };
        }
    }

    private static string FormatBytes(long bytes)
    {
        string[] suffixes = ["B", "KB", "MB", "GB", "TB"];
        double size = bytes;
        int suffix = 0;

        while (size >= 1024 && suffix < suffixes.Length - 1)
        {
            size /= 1024;
            suffix++;
        }

        return $"{size:0.##} {suffixes[suffix]}";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void VisibleCreateFile()
    {
        HasVisibleCreateFile = true;
        OnPropertyChanged();
    }
    public void NoVisibleCreateFile()
    {
        HasVisibleCreateFile = false;
        OnPropertyChanged();
    }
    public async Task CreateFileCommand()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(FileName))
                return;

            var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");
            await manager.CreateFile(CurrentDirectory.Path,FileName);
            CurrentDirectory = await manager.GetFiles(CurrentDirectory.Path);
            FileName = string.Empty;
        }
        finally
        {
            HasVisibleCreateFile = false; 
            OnPropertyChanged();
        }
    }
    public void VisibleCreateFolder()
    {
        HasVisibleCreateFolder = true;
        OnPropertyChanged();
    }
    public void NoVisibleCreateFolder()
    {
        HasVisibleCreateFolder = false;
        OnPropertyChanged();
    }
    public async Task CreateFolderCommand()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(FolderName))
                return;

            await CreateFolderAsync(FolderName);

            FolderName = string.Empty;
        }
        finally
        {
            HasVisibleCreateFolder = false; 
            OnPropertyChanged();
        }
    }

    private async Task CreateFolderAsync(string name)
    {
        var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");
        await manager.CreateFolder(CurrentDirectory.Path,name);
        CurrentDirectory = await manager.GetFiles(CurrentDirectory.Path);
    }
}