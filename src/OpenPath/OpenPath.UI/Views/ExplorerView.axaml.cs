using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using OpenPath.UI.Models;
using OpenPath.UI.ViewModels;

namespace OpenPath.UI.Views;

public partial class ExplorerView : UserControl
{
    public ExplorerView()
    {
        InitializeComponent();
    }

    private async void OnHomeClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(
                    @"Assets/SystemCommandsWindows.json");

                vm.CurrentDirectory =
                    await manager.GetFiles(await manager.GetArchive(manager.Archives.Profile));
            }
        }
        catch
        {
            // ignored
        }
    }
    
    private async void OnDocumentClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(
                    @"Assets/SystemCommandsWindows.json");

                vm.CurrentDirectory =
                    await manager.GetFiles(await manager.GetArchive(manager.Archives.Documents));
            }
        }
        catch
        {
            // ignored
        }
    }
    private async void OnDownloadClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(
                    @"Assets/SystemCommandsWindows.json");

                vm.CurrentDirectory =
                    await manager.GetFiles(await manager.GetArchive(manager.Archives.Downloads));
            }
        }
        catch
        {
            // ignored
        }
    }
    private async void OnFavoritesClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(
                    @"Assets/SystemCommandsWindows.json");

                vm.CurrentDirectory =
                    await manager.GetFiles(await manager.GetArchive(manager.Archives.Favorites));
            }
        }
        catch 
        {
            // ignored
        }
    }
    private async void OnPicturesClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(
                    @"Assets/SystemCommandsWindows.json");

                vm.CurrentDirectory =
                    await manager.GetFiles(await manager.GetArchive(manager.Archives.Pictures));
            }
        }
        catch 
        {
            // ignored
        }
    }
    
    private async void OnFolderClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: Directory folder })
            return;

        await OnFolderSelected(folder.Path);
    }

    private void OnFileClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: File file })
            return;
        
        OnFileSelected(file.Path);
    }

    private async Task OnFolderSelected(string folderPath)
    {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(
                    @"Assets/SystemCommandsWindows.json");

                vm.CurrentDirectory =
                    await manager.GetFiles(folderPath);
            }
    }

    private async void OnFileSelected(string filePath)
    {
        try
        {
            var manager = new ManagerCommands(
                @"Assets\SystemCommandsWindows.json");
            await manager.OpenFile(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            System.IO.File.AppendAllText(
                @"debug.txt",$"\n \t DEU EXECÇEÂOOO  : \n \t {e.Message}");

        }
        
    }
    
    private async void OnBackClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                if (string.IsNullOrEmpty(vm.Previous))
                    return;
                
                var manager = new ManagerCommands(
                    @"Assets\SystemCommandsWindows.json");

                if(!string.IsNullOrWhiteSpace(vm.Previous))
                    vm.CurrentDirectory =
                        await manager.GetFiles(vm.Previous);
            }
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    private async void OnForwardClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                if (string.IsNullOrEmpty(vm.Next))
                    return;
                
                var manager = new ManagerCommands(
                    @"Assets\SystemCommandsWindows.json");
                
                if(!string.IsNullOrWhiteSpace(vm.Next))
                    vm.CurrentDirectory =
                        await manager.GetFiles(vm.Next);

            }
        }
        catch (Exception ex)
        {
           
        }
    }
    
    //ACLKSDJLFÇSFÇFÇFÇFÇFÇFÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFSÇFFçç
    
    private async void OnOpenFolderClickMenuItem(object? sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem { DataContext: Directory folder })
            return;

        await OnFolderSelected(folder.Path);
    }
    
    private void OnPartitionClick(object? sender, RoutedEventArgs routedEventArgs)
    {
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
    }
    
    private void OnRenameClick(object? sender, RoutedEventArgs e)
    {
    }
    
    private async void OnRefreshClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(
                    @"Assets\SystemCommandsWindows.json");
                
                vm.CurrentDirectory =
                    await manager.GetFiles(vm.CurrentDirectory.Path);
            }
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
    
    private async void OnCopyPathClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        if (sender is MenuItem { DataContext: File file })
            if (topLevel?.Clipboard is not null)
            {
                await topLevel.Clipboard.SetTextAsync(file.Path);
            }
        
        if (sender is MenuItem { DataContext: Directory directory })
            if (topLevel?.Clipboard is not null)
            {
                await topLevel.Clipboard.SetTextAsync(directory.Path);
            }
    }
    
    private void OnNewFileClick(object? sender, RoutedEventArgs e)
    {
    }
    
    private void OnNewFolderClick(object? sender, RoutedEventArgs e)
    {
    }
}