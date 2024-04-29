using System;
using System.IO;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;

public class FileWatcherService
{
    private FileSystemWatcher _watcher;
    public void SetPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new InvalidOperationException("Watch folder path must be set.");
        }
        _watcher = new FileSystemWatcher(path)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            Filter = "*.*",
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _watcher.Created += OnCreated;
        // _watcher.Changed += OnChanged;
        _watcher.EnableRaisingEvents = true;
    }


    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        var imageBytes = File.ReadAllBytes(e.FullPath);
        var base64Image = Convert.ToBase64String(imageBytes);
        var timestamp = DateTime.Now;
        var message = $"{base64Image} | {timestamp}";
        Console.WriteLine(message);
        SendMessage(message);
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        var timestamp = DateTime.Now;
        var message = $"File changed: {e.FullPath} | {timestamp}";
        Console.WriteLine(message);
        SendMessage(message);
    }

    private void SendMessage(string message)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "fileChanges",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: "fileChanges",
                                 basicProperties: null,
                                 body: body);
        }
    }

}