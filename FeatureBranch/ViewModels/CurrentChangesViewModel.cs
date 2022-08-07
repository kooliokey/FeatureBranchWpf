using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FeatureBranch.ViewModels;

public class CurrentChangesViewModel : BaseViewModel, IDisposable
{
    public CurrentChangesViewModel(Repository repository)
    {
        _repository = repository;
        HandleFileSystemChange();

        _fileSystemWatcher = new FileSystemWatcher(_repository.Info.WorkingDirectory)
        {
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _createdDeletedOrChangedEventHandler = new FileSystemEventHandler(FileCreatedChangedOrDeleted);
        _renamedEventHandler = new RenamedEventHandler(FileRenamed);

        _fileSystemWatcher.Created += _createdDeletedOrChangedEventHandler;
        _fileSystemWatcher.Deleted += _createdDeletedOrChangedEventHandler;
        _fileSystemWatcher.Renamed += _renamedEventHandler;
        _fileSystemWatcher.Changed += _createdDeletedOrChangedEventHandler;
    }

    private readonly FileSystemEventHandler _createdDeletedOrChangedEventHandler;

    private readonly FileSystemWatcher _fileSystemWatcher;

    private readonly RenamedEventHandler _renamedEventHandler;

    private readonly Repository _repository;

    private IEnumerable<StatusEntryViewModel> _indexChanges;

    private IEnumerable<StatusEntryViewModel> _workingDirectoryChanges;

    public IEnumerable<StatusEntryViewModel> IndexChanges
    {
        get
        {
            return _indexChanges;
        }
        private set
        {
            _indexChanges = value;

            OnPropertyChanged(nameof(IndexChanges));
        }
    }

    public IEnumerable<StatusEntryViewModel> WorkingDirectoryChanges
    {
        get
        {
            return _workingDirectoryChanges;
        }
        private set
        {
            _workingDirectoryChanges = value;

            OnPropertyChanged(nameof(WorkingDirectoryChanges));
        }
    }

    public void Dispose()
    {
        _fileSystemWatcher.Created -= _createdDeletedOrChangedEventHandler;
        _fileSystemWatcher.Deleted -= _createdDeletedOrChangedEventHandler;
        _fileSystemWatcher.Renamed -= _renamedEventHandler;
        _fileSystemWatcher.Changed -= _createdDeletedOrChangedEventHandler;

        _fileSystemWatcher.Dispose();
    }

    public void FileCreatedChangedOrDeleted(object source, FileSystemEventArgs args) => HandleFileSystemChange();

    public void FileRenamed(object source, RenamedEventArgs args) => HandleFileSystemChange();

    public void HandleFileSystemChange()
    {
        WorkingDirectoryChanges = _repository.RetrieveStatus(new StatusOptions()
        {
            Show = StatusShowOption.WorkDirOnly,
            IncludeUntracked = true,
            DetectRenamesInWorkDir = true
        }).Select(x => new StatusEntryViewModel(x)).ToList();

        IndexChanges = _repository.RetrieveStatus(new StatusOptions()
        {
            Show = StatusShowOption.IndexOnly,
            IncludeUntracked = true,
            DetectRenamesInIndex = true
        }).Select(x => new StatusEntryViewModel(x)).ToList();
    }
}