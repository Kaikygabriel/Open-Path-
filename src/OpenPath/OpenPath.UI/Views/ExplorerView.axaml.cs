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
                var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");
                vm.CurrentDirectory = await manager.GetFiles(await manager.GetArchive(manager.Archives.Profile));
            }
        }
        catch
        {
        }
    }

    private async void OnDocumentClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");
                vm.CurrentDirectory = await manager.GetFiles(await manager.GetArchive(manager.Archives.Documents));
            }
        }
        catch
        {
        }
    }

    private async void OnDownloadClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");
                vm.CurrentDirectory = await manager.GetFiles(await manager.GetArchive(manager.Archives.Downloads));
            }
        }
        catch
        {
        }
    }

    private async void OnFavoritesClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");
                vm.CurrentDirectory = await manager.GetFiles(await manager.GetArchive(manager.Archives.Favorites));
            }
        }
        catch
        {
        }
    }

    private async void OnPicturesClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ExplorerViewModel vm)
            {
                var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");
                vm.CurrentDirectory = await manager.GetFiles(await manager.GetArchive(manager.Archives.Pictures));
            }
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

    public async Task OnFolderSelected(string folderPath)
    {
        if (DataContext is ExplorerViewModel vm)
        {
            var manager = new ManagerCommands(@"Assets/SystemCommandsWindows.json");
            vm.CurrentDirectory = await manager.GetFiles(folderPath);
        }
    }

    public async void OnFileSelected(string filePath)
    {
        try
        {
            var manager = new ManagerCommands(@"Assets\SystemCommandsWindows.json");
            await manager.OpenFile(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            System.IO.File.AppendAllText(@"debug.txt", $"\n \t DEU EXECÇEÂOOO  : \n \t {e.Message}");
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

                var manager = new ManagerCommands(@"Assets\SystemCommandsWindows.json");

                if (!string.IsNullOrWhiteSpace(vm.Previous))
                    vm.CurrentDirectory = await manager.GetFiles(vm.Previous);
            }
        }
        catch
        {
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

                var manager = new ManagerCommands(@"Assets\SystemCommandsWindows.json");

                if (!string.IsNullOrWhiteSpace(vm.Next))
                    vm.CurrentDirectory = await manager.GetFiles(vm.Next);
            }
        }
        catch
        {
        }
    }

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
                var manager = new ManagerCommands(@"Assets\SystemCommandsWindows.json");
                vm.CurrentDirectory = await manager.GetFiles(vm.CurrentDirectory.Path);
            }
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
        {
            System.IO.File.AppendAllText(@"debug.txt", $"\n \t ENTROU AQUI NO   OnNewFolderClick");

            vm.VisibleCreateFile();
        }
    }

    private void OnNewFolderClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
        {
            System.IO.File.AppendAllText(@"debug.txt", $"\n \t ENTROU AQUI NO   OnNewFolderClick");

            vm.VisibleCreateFolder();
        }

    }

    private async void CreateFolderCommand(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
        {
            System.IO.File.AppendAllText(@"debug.txt", $"\n \t ENTROU AQUI NO   OnNewFolderClick");

            await vm.CreateFolderCommand();
        }
    }
    private async void CancelCreateFolder(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
        {
            System.IO.File.AppendAllText(@"debug.txt", $"\n \t ENTROU AQUI NO   OnNewFolderClick");

            vm.NoVisibleCreateFolder();
        }
    }
    private async void CreateFileCommand(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
        {
            System.IO.File.AppendAllText(@"debug.txt", $"\n \t ENTROU AQUI NO   OnNewFolderClick");

            await vm.CreateFileCommand();
        }
    }
    private async void CancelCreateFile(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ExplorerViewModel vm)
        {
            vm.NoVisibleCreateFile();
        }
    }
}