using LibGit2Sharp;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FeatureBranch.ViewModels;

public class BranchesSidebarViewModel : BaseViewModel
{
    public BranchesSidebarViewModel(Repository repository)
    {
        _repository = repository;

        CurrentBranch = _repository.Head.FriendlyName;

        SetBranches(_repository.Branches);
    }

    private readonly Repository _repository;

    private ICommand? _createNewBranchCommand;

    private string _currentBranch;

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

    public BranchesListViewModel BugBranches { get; private set; }

    public BranchesListViewModel CherrypickBranches { get; private set; }

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

    public string CurrentBranch
    {
        get
        {
            return _currentBranch;
        }
        set
        {
            _currentBranch = value;

            OnPropertyChanged(nameof(CurrentBranch));
        }
    }

    public BranchesListViewModel FeatureBranches { get; private set; }

    public BranchesListViewModel LongLivedBranches { get; private set; }

    public BranchesListViewModel OtherBranches { get; private set; }

    private void CreateNewBranch()
    {
        MessageBox.Show("Create New Branch");
    }

    public void SetBranches(BranchCollection branches)
    {
        var bugBranches = new List<SingleBranchViewModel>();
        var featureBranches = new List<SingleBranchViewModel>();
        var cherrypickBranches = new List<SingleBranchViewModel>();
        var longLivedBranches = new List<SingleBranchViewModel>();
        var otherBranches = new List<SingleBranchViewModel>();

        foreach (var branch in branches)
        {
            if (LongLivedBranchNames.Contains(branch.FriendlyName))
            {
                longLivedBranches.Add(new SingleBranchViewModel(branch));
            }
            else if (FeatureBranchPattern.IsMatch(branch.FriendlyName))
            {
                featureBranches.Add(new SingleBranchViewModel(branch));
            }
            else if (BugBranchPattern.IsMatch(branch.FriendlyName))
            {
                bugBranches.Add(new SingleBranchViewModel(branch));
            }
            else if (CherrypickBranchPattern.IsMatch(branch.FriendlyName))
            {
                cherrypickBranches.Add(new SingleBranchViewModel(branch));
            }
            else
            {
                otherBranches.Add(new SingleBranchViewModel(branch));
            }
        }

        BugBranches = new BranchesListViewModel(bugBranches);
        FeatureBranches = new BranchesListViewModel(featureBranches);
        CherrypickBranches = new BranchesListViewModel(cherrypickBranches);
        LongLivedBranches = new BranchesListViewModel(longLivedBranches);
        OtherBranches = new BranchesListViewModel(otherBranches);
    }
}