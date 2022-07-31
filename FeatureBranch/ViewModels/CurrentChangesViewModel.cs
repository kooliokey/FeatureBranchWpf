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

    private Repository _repository;

    private IEnumerable<StatusEntryViewModel> _status;

    public IEnumerable<StatusEntryViewModel> Status
    {
        get
        {
            return _status;
        }
        private set
        {
            _status = value;
            OnPropertyChanged(nameof(Status));
        }
    }

    private void HandleFileSystemChange()
    {
        var status = _repository.RetrieveStatus();
        Status = status
            .Select(x => new StatusEntryViewModel(x))
            .ToList();
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
}