using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using OpenPath.UI.Models;
using OpenPath.UI.ViewModels;

namespace OpenPath.UI.Views;

public partial class ExplorerView : UserControl
{
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
}