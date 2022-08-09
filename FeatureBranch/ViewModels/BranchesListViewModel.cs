using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace FeatureBranch.ViewModels;

public class BranchesListViewModel : BaseViewModel
{
    public BranchesListViewModel(IEnumerable<SingleBranchViewModel> branches)
    {
        Branches = branches;
    }

    public BranchesListViewModel()
    {
    }

    private IEnumerable<SingleBranchViewModel> _branches;

    private RelayCommand _doubleClickCommand;

    public IEnumerable<SingleBranchViewModel> Branches
    {
        get
        {
            return _branches;
        }
        private set
        {
            _branches = value;

            OnPropertyChanged(nameof(Branches));
        }
    }

    public ICommand DoubleClickBranch
    {
        get
        {
            if (_doubleClickCommand is null)
            {
                _doubleClickCommand = new RelayCommand(ChangeBranch);
            }

            return _doubleClickCommand;
        }
    }

    public string Name { get; set; } = "TEST";

    private void ChangeBranch()
    {
        MessageBox.Show("Change Branch");
    }
}