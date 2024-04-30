using Microsoft.EntityFrameworkCore;
using ResourceWatcher.Data;
using ResourceWatcher.Models;

namespace ResourceWatcher.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SaveMessageAsync(string message)
        {
            var img = new ImageModel
            {
                Type = message.Split(" | ")[0],
                Url = message.Split(" | ")[1],
                Timestamp = message.Split(" | ")[2],
            };

            _context.Images.Add(img);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ImageModel>> GetAllMessagesAsync()
        {
            return await _context.Images.ToListAsync();
        }
    }
}