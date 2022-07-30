using System.ComponentModel;

namespace FeatureBranch.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}