using LibGit2Sharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace FeatureBranch.ViewModels;

public class BranchesViewModel : BaseViewModel
{
    public BranchesViewModel(BranchCollection branches)
    {
        SetBranches(branches);
    }

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

    public ObservableCollection<Branch>? BugBranches { get; private set; }

    public ObservableCollection<Branch>? CherrypickBranches { get; private set; }

    public ObservableCollection<Branch>? FeatureBranches { get; private set; }

    public ObservableCollection<Branch>? LongLivedBranches { get; private set; }

    public void SetBranches(BranchCollection branches)
    {
        BugBranches = new ObservableCollection<Branch>(branches.Where(x => BugBranchPattern.IsMatch(x.FriendlyName)).ToList());
        FeatureBranches = new ObservableCollection<Branch>(branches.Where(x => FeatureBranchPattern.IsMatch(x.FriendlyName)).ToList());
        CherrypickBranches = new ObservableCollection<Branch>(branches.Where(x => CherrypickBranchPattern.IsMatch(x.FriendlyName)).ToList());
        LongLivedBranches = new ObservableCollection<Branch>(branches.Where(x => LongLivedBranchNames.Contains(x.FriendlyName.ToLowerInvariant())).ToList());
    }
}