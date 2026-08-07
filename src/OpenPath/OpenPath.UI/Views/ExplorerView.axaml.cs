using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Microsoft.VisualBasic;
using OpenPath.UI.Enuns;
using OpenPath.UI.Models;
using OpenPath.UI.ViewModels;

namespace OpenPath.UI.Views;

public partial class ExplorerView : UserControl
{
    private string _pathMove;
    private bool _isFileMove;
    
    private bool _isDragging;
    private string? _dragPath;
    private bool _dragIsFile;


    private Border? _dragTarget;
    private string _pathRename;
    private bool _isFileRename;
    
    private ManagerCommands _manager;
    public ExplorerView()
    {
        _manager = new ManagerCommands();
        InitializeComponent();
    }
  
    private void OnHomeClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
                vm.CurrentDirectory = _manager.GetFiles(Archives.Profile);
        }
        catch
        {
        }
    }

    private void OnDocumentClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
                vm.CurrentDirectory = _manager.GetFiles(Archives.Documents);
        }
        catch
        {
        }
    }

    private void OnDownloadClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
                vm.CurrentDirectory = _manager.GetFiles(Archives.Downloads);
        }
        catch
        {
        }
    }

    private void OnFavoritesClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
                vm.CurrentDirectory =  _manager.GetFiles(Archives.Favorites);
        }
        catch
        {
        }
    }

    private void OnPicturesClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
                vm.CurrentDirectory = _manager.GetFiles(Archives.Pictures);
        }
        catch
        {
        }
    }

    private void OnCloseSelect(object? sender, RoutedEventArgs e)
    {
        if ( DataContext is ExplorerViewModel vm)
        {
            vm.SelectedDirectory = null;
            vm.SelectedFile = null;
        }
    }
    
    // Clique único: seleciona a pasta no painel da direita
    private void OnSelectFolder(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: Directory directory } &&
            DataContext is ExplorerViewModel vm)
        {
            vm.SelectedDirectory = directory;
        }
    }

    // Clique único: seleciona o arquivo no painel da direita
    private void OnSelectFile(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: File file } &&
            DataContext is ExplorerViewModel vm)
        {
            vm.SelectedFile = file;
        }
    }

    private async void OnFolderClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: ExplorerViewModel vm } )
            OnFolderSelected(vm.SelectedPath);
        if (sender is Button { DataContext: Directory folder} )
            OnFolderSelected(folder.Path);
    }

    private void OnFileClick(object? sender, RoutedEventArgs e)
    {
        
        if (sender is Button { DataContext: ExplorerViewModel vm } )
        {
            if (System.IO.Directory.Exists(vm.SelectedPath))
                OnFolderSelected(vm.SelectedPath);
            else
                _manager.OpenFile(vm.SelectedPath);
        }
        if (sender is Button { DataContext: File file } )
        {
            _manager.OpenFile(file.Path);
        }
    }

    public void OnFolderSelected(string folderPath)
    {
        if (DataContext is ExplorerViewModel vm)
            vm.CurrentDirectory =  _manager.GetFiles(folderPath);
    }

    public void OnFolderSelectedOrderByDate(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
        {
            vm.ModeConfiguration.ModeOrder = EOrderMode.Data; 
            vm.CurrentDirectory =  _manager.GetFiles(vm.CurrentDirectory.Path);
        }
    }
    
    public void OnFolderSelectedOrderByLenght(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
        {
            vm.ModeConfiguration.ModeOrder = EOrderMode.Lenght; 
            vm.CurrentDirectory =  _manager.GetFiles(vm.CurrentDirectory.Path);
        }
    }
        public void OnFolderSelectedOrderByTitle(object? sender, RoutedEventArgs e)
        {
            if (DataContext is ExplorerViewModel vm)
            {
                vm.ModeConfiguration.ModeOrder = EOrderMode.Name; 
                vm.CurrentDirectory =  _manager.GetFiles(vm.CurrentDirectory.Path);
            }
        }
        
    public void OnFileSelected(string filePath)
    {
        try
        {
            _manager.OpenFile(filePath);
        }
        catch (Exception e)
        {
            System.IO.File.AppendAllText(@"debug.txt", $"\n \t DEU EXECÇEÂOOO  : \n \t {e.Message}");
        }
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                if (string.IsNullOrEmpty(vm.Previous))
                    return;

                if (!string.IsNullOrWhiteSpace(vm.Previous))
                    vm.CurrentDirectory =  _manager.GetFiles(vm.Previous);
            }
        }
        catch
        {
        }
    }

    private void OnForwardClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                if (string.IsNullOrEmpty(vm.Next))
                    return;

                if (!string.IsNullOrWhiteSpace(vm.Next))
                    vm.CurrentDirectory =  _manager.GetFiles(vm.Next);
            }
        }
        catch
        {
        }
    }

    private void OnOpenFolderClickMenuItem(object? sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem { DataContext: Directory folder })
            return;

        OnFolderSelected(folder.Path);
    }

    private void OnPartitionClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        if (sender is Button { DataContext: Partition drive } )
        {
            OnFolderSelected(drive.PathRootDirectory);
        }
    }

    private void OnFileContextRequested(object? sender, ContextRequestedEventArgs e)
    {
    }

    private void OnFolderContextRequested(object? sender, ContextRequestedEventArgs e)
    {
    }

    private void OnBackgroundContextRequested(object? sender, ContextRequestedEventArgs e)
    {
    }

    private void OnOpenFileClickMenuItem(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: File file })
        {
            OnFileSelected(file.Path);
        }
    }

    private void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is MenuItem { DataContext: File file })
            {
                System.IO.File.Delete(file.Path);
                OnRefreshClick(sender,e);
            }
            if (sender is MenuItem { DataContext: Directory directory })
            {
                System.IO.Directory.Delete(directory.Path, true);
                OnRefreshClick(sender,e);
            }
        }
        catch
        {
            // ignored
        }
    }

    private void OnRenameClick(object? sender, RoutedEventArgs e)
    {
    }

    private void OnRefreshClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm )
                vm.CurrentDirectory = _manager.GetFiles(vm.CurrentDirectory.Path);
            
        }
        catch
        {
        }
    }

    private async void OnCopyPathClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        if (sender is MenuItem { DataContext: File file } && topLevel?.Clipboard is not null)
        {
            await topLevel.Clipboard.SetTextAsync(file.Path);
        }

        if (sender is MenuItem { DataContext: Directory directory } && topLevel?.Clipboard is not null)
        {
            await topLevel.Clipboard.SetTextAsync(directory.Path);
        }
        
        if (sender is Button { DataContext: File fileButton } && topLevel?.Clipboard is not null)
        {
            await topLevel.Clipboard.SetTextAsync(fileButton.Path);
        }
        
        if (sender is Button { DataContext: Directory directoryButton } && topLevel?.Clipboard is not null)
        {
            await topLevel.Clipboard.SetTextAsync(directoryButton.Path);
        }
    }

    private void OnNewFileClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
            vm.VisibleCreateFile();
        
    }

    private void OnNewFolderClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
            vm.VisibleCreateFolder();
    }
    
    private void OnIconsMode(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
            vm.SetIconMode();
    }

    private void OnDetailsMode(object? sender, RoutedEventArgs e)
    {
          if (DataContext is ExplorerViewModel vm)
            vm.SetDetailsMode();
    }
    
    
    private async void CreateFolderCommand(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
            await vm.CreateFolderCommand();
    }
    private async void CancelCreateFolder(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
            vm.NoVisibleCreateFolder();
    }
    private async void CreateFileCommand(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
            await vm.CreateFileCommand();
    }
    private async void CancelCreateFile(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
            vm.NoVisibleCreateFile();
    }
    
    private void OnOpenRename(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: File file })
        {
            _pathRename =  file.Path;
            _isFileRename = true;
        }
        
        if (sender is MenuItem { DataContext: Directory directory })
        {
            _pathRename =  directory.Path;
            _isFileRename = false;
        }


        if (DataContext is ExplorerViewModel vm)
        {
            if (vm.SelectedDirectory is not null && string.IsNullOrEmpty(_pathRename))
            {
                _pathRename = vm.SelectedDirectory.Path;
                _isFileRename = false;
            }

            if (vm.SelectedFile is not null && string.IsNullOrEmpty(_pathRename))
            {
                _pathRename = vm.SelectedFile.Path;
                _isFileRename = true;
            }

            vm.VisibleRename(_pathRename.Split(new[] { '/', '\\' }).ToList().Last());
        }
    }

    public  void CancelMove(object? sender, RoutedEventArgs e)
    {
        _pathMove = "";
        if (DataContext is ExplorerViewModel vm)
            vm.NoVisibleMove();
    }
    
    private void Rename(object? sender, RoutedEventArgs e)
    
    {
        if (DataContext is ExplorerViewModel vm && !string.IsNullOrEmpty(vm.NewNameRename))
        {
            var oldPath = _pathRename;
            
            var parentFolder = System.IO.Path.GetDirectoryName(oldPath);
            if (parentFolder == null) return;

            var result = System.IO.Path.Combine(parentFolder, vm.NewNameRename);
        
            if (_isFileRename)
            {
                System.IO.File.Move(oldPath, result);
            }
            else
            {
                System.IO.Directory.Move(oldPath, result);
            }
            
            _pathRename = string.Empty;
            vm.NoVisibleRename(_pathRename);
        }
    }
    public void CancelRename(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
            vm.NoVisibleRename(_pathRename);
    }
    public void SelectedFolderInMove(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: Directory directory })
        {
            if (DataContext is ExplorerViewModel vm)
            {
                vm.CurrentPathMove = System.IO.Path.Combine(vm.CurrentPathMove,directory.Name);
            }
        }

    }
    
    private void OnOpenMove(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: File file })
        {
            _pathMove =  file.Path;
            _isFileMove = true;
        }
        
        if (sender is MenuItem { DataContext: Directory directory })
        {
            _pathMove =  directory.Path;
            _isFileMove = false;
        }


        if (DataContext is ExplorerViewModel vm)
        {
            if (vm.SelectedDirectory is not null && string.IsNullOrEmpty(_pathMove))
            {
                _pathMove = vm.SelectedDirectory.Path;
                _isFileMove = false;
            }

            if (vm.SelectedFile is not null && string.IsNullOrEmpty(_pathMove))
            {
                _pathMove = vm.SelectedFile.Path;
                _isFileMove = true;
            }
            System.IO.File.AppendAllText(
                @"debug.txt",
                $" \t \n  NOME DO MOVE {_pathMove} \t \t ");
            vm.VisibleMove(_pathMove);
        }

    }
    
    private async void MoveFile(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm && !string.IsNullOrEmpty(vm.CurrentPathMove))
            {
                
                if (_isFileRename)
                {
                    System.IO.File.Move(_pathMove, System.IO.Path.Combine(vm.CurrentPathMove,System.IO.Path.GetFileName(_pathMove)));
                }
                else
                {
                    System.IO.Directory.Move(_pathMove, System.IO.Path.Combine(vm.CurrentPathMove,System.IO.Path.GetFileName(_pathMove)));
                }
            
                _pathMove = string.Empty;
                vm.NoVisibleMove();
            }
        }
        catch (Exception exception)
        {
            await System.IO.File.AppendAllTextAsync(@"debug.txt", $"Class : ExplorerView.axml.cs - Method :MoveFile {exception.Message}");
        }
    }       
}