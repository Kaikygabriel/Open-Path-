using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using OpenPath.UI.Configurations;
using OpenPath.UI.Enuns;
using OpenPath.UI.Models;

namespace OpenPath.UI.ViewModels;

public class ExplorerViewModel : INotifyPropertyChanged
{
    private ManagerCommands _manager;
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
    
    private bool _hasVisibleViewModeOptions;

    public bool HasVisibleViewModeOptions
    {
        get => _hasVisibleViewModeOptions;
        set
        {
            if (_hasVisibleViewModeOptions == value)
                return;

            _hasVisibleViewModeOptions = value;
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
    
    private bool _hasVisibleMove;

    public bool HasVisibleMove
    {
        get => _hasVisibleMove;
        set
        {
            if (_hasVisibleMove == value)
                return;

            _hasVisibleMove = value;
            OnPropertyChanged();
        }
    }
    
    private bool _hasVisibleRename;

    public bool HasVisibleRename
    {
        get => _hasVisibleRename;
        set
        {
            if (_hasVisibleRename == value)
                return;

            _hasVisibleRename = value;
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

    private string _currentPathMove = "";

    public string CurrentPathMove
    {
        get => _currentPathMove;
        set
        {
            if (_currentPathMove == value)
                return;

            _currentPathMove = value;
            OnPropertyChanged();
        }
    }

    private string _newNameRename = "";

    public string NewNameRename
    {
        get => _newNameRename;
        set
        {
            if (_newNameRename == value)
                return;

            _newNameRename = value;
            OnPropertyChanged();
        }
    }    
    public string FileName { get; set; }
    public string FolderName { get; set; }
    
    public string Next { get; set; } = string.Empty;
    public string Previous { get; set; } = string.Empty;

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

            if (ModeConfiguration.ModeOrder is EOrderMode.Data)
            {
                _currentDirectory.Directories = _currentDirectory.Directories.OrderByDescending(x => x.Date).ToList();
                _currentDirectory.Files = _currentDirectory.Files.OrderByDescending(x => x.LastWriteTime).ToList();
            }
            if (ModeConfiguration.ModeOrder is EOrderMode.Lenght)
            {
                _currentDirectory.Files = _currentDirectory.Files.OrderBy(x => x.Lenght).ToList();
            }
            if (ModeConfiguration.ModeOrder is EOrderMode.Name)
            {
                _currentDirectory.Directories = _currentDirectory.Directories.OrderBy(x => x.Name).ToList();
                _currentDirectory.Files = _currentDirectory.Files.OrderBy
                    (x => x.Name).ToList();
            }
            
            if (OperatingSystem.IsWindows())
            {
                var path = _currentDirectory.Path.Split('\\').ToList();
                if (path.Count > 0)
                    path.Remove(path.Last());

                Previous = path.Count > 0 ? string.Join('\\', path) : string.Empty;
            }
            else
            {
                var path = _currentDirectory.Path.Split('/').ToList();
                if (path.Count > 0)
                    path.Remove(path.Last());

                Previous = path.Count > 0 ? string.Join('/', path) : string.Empty;
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
    private ModeConfiguration _modeConfiguration;

    public ModeConfiguration ModeConfiguration
    {
        get => _modeConfiguration;
        set
        {
            try
            {
                if (_modeConfiguration == value)
                    return;

                _modeConfiguration = value;

                if(!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(ConfigurationKey.PathConfigurationMode)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ConfigurationKey.PathConfigurationMode)!);

                System.IO.File.WriteAllText(ConfigurationKey.PathConfigurationMode,
                    JsonSerializer.Serialize(_modeConfiguration));

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsIconMode));
                OnPropertyChanged(nameof(IsDetailsMode));
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText(
                    @"debug.txt",
                    $" \t \n  exceção {e.Message} {e.StackTrace} \t \t ");
            }
        }
    }
    public bool IsIconMode => ModeConfiguration.ModeView == EModeView.Icons;

    public bool IsDetailsMode => ModeConfiguration.ModeView == EModeView.Details;
    public void SetIconMode()
    {
        ModeConfiguration = new ModeConfiguration()
        {
            ModeOrder = ModeConfiguration.ModeOrder,
            ModeView = EModeView.Icons
        };
    }

    public void SetDetailsMode()
    {
        ModeConfiguration = new ModeConfiguration()
        {
            ModeOrder = ModeConfiguration.ModeOrder,
            ModeView = EModeView.Details
        };
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

    public Bitmap SelectedIcon =>
        SelectedDirectory is not null ? new Bitmap("/Assets/directory.png") :
        SelectedFile is not null ? SelectedFile.Icon :
        new Bitmap("");

    public string SelectedPrimaryInfo =>
        SelectedDirectory is not null
            ? $"{SelectedDirectory.Directories.Count} subpastas • {SelectedDirectory.Files.Count} arquivos"
            : SelectedFile is not null
                ? $"Tamanho: {SelectedFile.Lenght}"
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
                $" \t \n  Começou a carregar \t \t ");

            await ApplyModeConfiguration();
            _manager = new ManagerCommands();

            CurrentDirectory = _manager.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                
            Partitions =  _manager.GetPartition();
            await System.IO.File.AppendAllTextAsync(
                @"debug.txt",
                $" \t \n PASSOU DO LOAD DO EXPLORERVIEWMODEL \t \t ");

        }
        catch (Exception e)
        {
            await System.IO.File.AppendAllTextAsync(
                @"debug.txt",
                $" \t \n  exceção {e.Message} {e.StackTrace} \t \t ");
            
            CurrentDirectory = new Directory
            {
                Name = "Erro ao carregar"
            };
        }
    }

    public async Task ApplyModeConfiguration()
    {
        try
        {
            if(!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(ConfigurationKey.PathConfigurationMode)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ConfigurationKey.PathConfigurationMode)!);
            
            var modeDefault =  EModeView.Icons;
            var modeConfiguration = JsonSerializer.Deserialize<ModeConfiguration>(await System.IO.File.ReadAllTextAsync(ConfigurationKey.PathConfigurationMode));
            
            if (modeConfiguration is null)
            {
                modeConfiguration = new ModeConfiguration()
                {
                    ModeView = modeDefault,
                    ModeOrder = EOrderMode.Name
                };
            }
            ModeConfiguration = modeConfiguration;
        }
        catch(Exception e )
        {
            ModeConfiguration = new ModeConfiguration()
            {
                ModeView = EModeView.Icons,
                ModeOrder = EOrderMode.Name
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

            await _manager.CreateFile(CurrentDirectory.Path,FileName);
            CurrentDirectory = _manager.GetFiles(CurrentDirectory.Path);
            FileName = string.Empty;
        }
        finally
        {
            HasVisibleCreateFile = false; 
            OnPropertyChanged();
        }
    }
    public void VisibleViewModeOptions()
    {
        HasVisibleViewModeOptions = true;
        OnPropertyChanged();
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
    
    public void VisibleMove(string path)
    {
        CurrentPathMove = path;
        HasVisibleMove = true;
        OnPropertyChanged();
    }
    public void NoVisibleMove()
    {
        CurrentPathMove = "";
        HasVisibleMove = false;
        CurrentDirectory = _manager.GetFiles(CurrentDirectory.Path); 
        OnPropertyChanged();
    }
    public void VisibleRename(string path)
    {
        NewNameRename = path;
        HasVisibleRename = true;
        OnPropertyChanged();
    }
    public void NoVisibleRename(string path)
    {
        NewNameRename = path;
        HasVisibleRename = false;
        CurrentDirectory = _manager.GetFiles(CurrentDirectory.Path); 
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
        await _manager.CreateFolder(CurrentDirectory.Path,name);
        CurrentDirectory = _manager.GetFiles(CurrentDirectory.Path);
    }
    
}