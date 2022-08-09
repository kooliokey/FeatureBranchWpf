using LibGit2Sharp;

namespace FeatureBranch.ViewModels;

public class SingleBranchViewModel : BaseViewModel
{
    public SingleBranchViewModel(Branch branch)
    {
        FriendlyName = branch.FriendlyName;
        IsCurrentRepositoryHead = branch.IsCurrentRepositoryHead;
    }

    private string _currentBranchIcon;

    private string _friendlyName;

    private bool _isCurrentRepositoryHead;

    public string CurrentBranchIcon
    {
        get
        {
            return _currentBranchIcon;
        }

        private set
        {
            _currentBranchIcon = value;

            OnPropertyChanged(nameof(CurrentBranchIcon));
        }
    }

    public string FriendlyName
    {
        get
        {
            return _friendlyName;
        }
        private set
        {
            _friendlyName = value;

            OnPropertyChanged(nameof(FriendlyName));
        }
    }

    public bool IsCurrentRepositoryHead
    {
        get
        {
            return _isCurrentRepositoryHead;
        }
        private set
        {
            _isCurrentRepositoryHead = value;

            CurrentBranchIcon = _isCurrentRepositoryHead
                ? "/Resources/Images/star.png"
                : string.Empty;

            OnPropertyChanged(nameof(IsCurrentRepositoryHead));
        }
    }
}