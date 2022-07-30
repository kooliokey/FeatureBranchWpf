namespace FeatureBranch.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    public MainWindowViewModel()
    {
        RepositoryViewModel = new();
    }

    public RepositoryViewModel RepositoryViewModel { get; }
}