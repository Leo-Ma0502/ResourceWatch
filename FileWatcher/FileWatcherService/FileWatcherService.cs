using System;
using System.IO;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;

public class FileWatcherService
{
    private readonly FileSystemWatcher _watcher;

    public FileWatcherService(IConfiguration configuration)
    {
        string _pathToWatch = "/Users/yehengma/Projects/ResourceWatch/FileWatcher/FileWatcherService/test";
        if (string.IsNullOrEmpty(_pathToWatch))
        {
            throw new InvalidOperationException("Watch folder path must be set.");
        }
        _watcher = new FileSystemWatcher(_pathToWatch)
        {
            NotifyFilter = NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName,
            Filter = "*.*",
            IncludeSubdirectories = true
        };

        _watcher.Created += OnCreated;
        _watcher.Changed += OnChanged;
        _watcher.EnableRaisingEvents = true;
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"File created: {e.FullPath}");
        SendMessage($"File created: {e.FullPath}");
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"File changed: {e.FullPath}");
        SendMessage($"File changed: {e.FullPath}");
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