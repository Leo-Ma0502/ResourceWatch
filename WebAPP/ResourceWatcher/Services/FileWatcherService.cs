using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ResourceWatcher.Services
{
    public class FileWatcherService : IFileWatcherService
    {
        private readonly HttpClient _httpClient;

        public FileWatcherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SetWatchPathAsync(string path)
        {
            try
            {
                var response = await _httpClient.PostAsync("/setPath", new StringContent(path, Encoding.UTF8, "application/json"));
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            return false;

        }
    }
}