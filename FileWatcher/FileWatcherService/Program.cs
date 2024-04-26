using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration.Sources.Clear();
                configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<FileWatcherService>();
            })
            .Build();

        var service = host.Services.GetRequiredService<FileWatcherService>();
        host.Run();
    }
}