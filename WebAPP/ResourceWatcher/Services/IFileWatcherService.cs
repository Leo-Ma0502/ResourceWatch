namespace ResourceWatcher.Services
{
    public interface IFileWatcherService
    {
        Task<bool> SetWatchPathAsync(string path);
    }
}