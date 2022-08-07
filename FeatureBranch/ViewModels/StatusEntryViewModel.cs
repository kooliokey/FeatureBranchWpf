using LibGit2Sharp;

namespace FeatureBranch.ViewModels;

public class StatusEntryViewModel : BaseViewModel
{
    public StatusEntryViewModel(StatusEntry statusEntry)
    {
        if (statusEntry.State == FileStatus.RenamedInIndex)
        {
            FilePathDisplay = $"{statusEntry.HeadToIndexRenameDetails.OldFilePath} -> {statusEntry.HeadToIndexRenameDetails.NewFilePath}";
        }
        else if (statusEntry.State == FileStatus.RenamedInWorkdir)
        {
            FilePathDisplay = $"{statusEntry.IndexToWorkDirRenameDetails.OldFilePath} -> {statusEntry.IndexToWorkDirRenameDetails.NewFilePath}";
        }
        else
        {
            FilePathDisplay = statusEntry.FilePath;
        }

        State = statusEntry.State;
    }

    public string FilePathDisplay { get; }

    public FileStatus State { get; }

    public string StateHint
    {
        get
        {
            switch (State)
            {
                case FileStatus.NewInIndex:
                case FileStatus.NewInWorkdir:
                    return "new";

                case FileStatus.DeletedFromIndex:
                case FileStatus.DeletedFromWorkdir:
                    return "deleted";

                case FileStatus.RenamedInIndex:
                case FileStatus.RenamedInWorkdir:
                    return "renamed";

                case FileStatus.ModifiedInIndex:
                case FileStatus.ModifiedInWorkdir:
                    return "modified";

                case FileStatus.TypeChangeInIndex:
                case FileStatus.TypeChangeInWorkdir:
                    return "type change";

                case FileStatus.Ignored:
                    return "ignored";

                case FileStatus.Conflicted:
                    return "conflict";

                case FileStatus.Unaltered:
                case FileStatus.Nonexistent:
                case FileStatus.Unreadable:
                default:
                    return string.Empty;
            }
        }
    }

    public string StateIcon
    {
        get
        {
            switch (State)
            {
                case FileStatus.NewInIndex:
                case FileStatus.NewInWorkdir:
                    return "/Resources/Images/plus.png";

                case FileStatus.DeletedFromIndex:
                case FileStatus.DeletedFromWorkdir:
                    return "/Resources/Images/close.png";

                case FileStatus.RenamedInIndex:
                case FileStatus.RenamedInWorkdir:
                    return "/Resources/Images/rename.png";

                case FileStatus.ModifiedInIndex:
                case FileStatus.ModifiedInWorkdir:
                    return "/Resources/Images/change.png";

                default:
                    return string.Empty;
            }
        }
    }
}