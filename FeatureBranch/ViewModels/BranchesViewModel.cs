using LibGit2Sharp;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FeatureBranch.ViewModels;

public class BranchesViewModel : BaseViewModel
{
    public BranchesViewModel(BranchCollection branches)
    {
        BugBranches = new ObservableCollection<Branch>();
        FeatureBranches = new ObservableCollection<Branch>();
        CherrypickBranches = new ObservableCollection<Branch>();
        LongLivedBranches = new ObservableCollection<Branch>();
        OtherBranches = new ObservableCollection<Branch>();

        SetBranches(branches);
    }

    private ICommand? _createNewBranchCommand;

    public static readonly Regex BugBranchPattern = new(@"bug\/kroche\/([0-9]+).*");

    public static readonly Regex CherrypickBranchPattern = new(@"cherrypick\/kroche\/([0-9A-z]+).*");

    public static readonly Regex FeatureBranchPattern = new(@"feature\/kroche\/([0-9]+).*");

    public static readonly IReadOnlyCollection<string> LongLivedBranchNames = new[]
    {
        "master",
        "main",
        "develop",
        "test",
        "next-release",
        "release/next"
    };

    public static readonly IReadOnlyCollection<string> PrioritizedDefaultNewBranch = new[]
    {
        "master",
        "main",
        "next-release",
        "release/next"
    };

    public ObservableCollection<Branch> BugBranches { get; private set; }

    public ObservableCollection<Branch> CherrypickBranches { get; private set; }

    public ICommand CreateNewBranchCommand
    {
        get
        {
            if (_createNewBranchCommand is null)
            {
                _createNewBranchCommand = new RelayCommand(CreateNewBranch);
            }

            return _createNewBranchCommand;
        }
    }

    public ObservableCollection<Branch> FeatureBranches { get; private set; }

    public ObservableCollection<Branch> LongLivedBranches { get; private set; }

    public ObservableCollection<Branch> OtherBranches { get; private set; }

    private void CreateNewBranch()
    {
        MessageBox.Show("Create New Branch");
    }

    public void SetBranches(BranchCollection branches)
    {
        LongLivedBranches.Clear();
        FeatureBranches.Clear();
        BugBranches.Clear();
        CherrypickBranches.Clear();
        OtherBranches.Clear();

        foreach (var branch in branches)
        {
            if (LongLivedBranchNames.Contains(branch.FriendlyName))
            {
                LongLivedBranches.Add(branch);
            }
            else if (FeatureBranchPattern.IsMatch(branch.FriendlyName))
            {
                FeatureBranches.Add(branch);
            }
            else if (BugBranchPattern.IsMatch(branch.FriendlyName))
            {
                BugBranches.Add(branch);
            }
            else if (CherrypickBranchPattern.IsMatch(branch.FriendlyName))
            {
                CherrypickBranches.Add(branch);
            }
            else
            {
                OtherBranches.Add(branch);
            }
        }
    }
}