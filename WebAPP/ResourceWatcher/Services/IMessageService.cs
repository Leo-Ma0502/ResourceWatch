using ResourceWatcher.Models;

namespace ResourceWatcher.Services
{
    public interface IMessageService
    {
        Task SaveMessageAsync(string message);
        Task<List<ImageModel>> GetAllMessagesAsync();
    }
}