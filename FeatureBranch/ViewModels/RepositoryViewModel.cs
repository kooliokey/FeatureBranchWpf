using LibGit2Sharp;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Input;

namespace FeatureBranch.ViewModels;

public class RepositoryViewModel : BaseViewModel
{
    private string? _currentDirectory;

    private Repository? _repository;

    private ICommand? _selectDirectoryCommand;

    public BranchesViewModel? BranchesViewModel { get; private set; }

    public string CurrentDirectoryDisplay => _currentDirectory is null
        ? "Select A Directory"
        : _currentDirectory;

    public bool? IsValidRepository { get; private set; }

    public ICommand SelectDirectoryCommand
    {
        get
        {
            if (_selectDirectoryCommand is null)
            {
                _selectDirectoryCommand = new RelayCommand(SelectDirectory);
            }

            return _selectDirectoryCommand;
        }
    }

    private void CheckGit()
    {
        IsValidRepository = Repository.IsValid(_currentDirectory);

        if (IsValidRepository.Value)
        {
            _repository = new Repository(_currentDirectory);
            BranchesViewModel = new BranchesViewModel(_repository.Branches);
            OnPropertyChanged(nameof(BranchesViewModel));
        }
    }

    public void SelectDirectory()
    {
        using var dialog = new CommonOpenFileDialog
        {
            IsFolderPicker = true
        };

        var userAction = dialog.ShowDialog();
        if (userAction == CommonFileDialogResult.Ok)
        {
            _currentDirectory = dialog.FileName;

            CheckGit();

            OnPropertyChanged(nameof(CurrentDirectoryDisplay));
        }
    }
}