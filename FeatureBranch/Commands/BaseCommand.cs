using System;
using System.Windows.Input;

namespace FeatureBranch.Commands;

public abstract class BaseCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    protected void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(null, EventArgs.Empty);
    }

    public virtual bool CanExecute(object? parameter) => true;

    public abstract void Execute(object? parameter);
}