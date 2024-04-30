using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

class Program
{
    public static void Main(string[] args)
    {
        static int GetAvailablePort(int startingPort)
        {
            int port = startingPort;
            bool isAvailable = false;

            TcpListener listener = null;

            while (!isAvailable)
            {
                try
                {
                    listener = new TcpListener(IPAddress.Loopback, port);
                    listener.Start();
                    isAvailable = true;
                }
                catch (SocketException)
                {
                    port++;
                }
                finally
                {
                    listener?.Stop();
                }
            }

            return port;
        }

        Console.WriteLine("Please enter a port to start the web server:");
        int userPort;
        while (!int.TryParse(Console.ReadLine(), out userPort) || userPort < 1024 || userPort > 65535)
        {
            Console.WriteLine("Invalid port. Please enter a valid port number (1024-65535):");
        }

        int availablePort = GetAvailablePort(userPort);

        if (availablePort != userPort)
        {
            Console.WriteLine($"Port {userPort} is not available. Using next available port: {availablePort}");
        }
        else
        {
            Console.WriteLine($"Using port {availablePort}");
        }

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapPost("/setPath", async context =>
                        {
                            var fileWatcherService = context.RequestServices.GetRequiredService<FileWatcherService>();

                            using (var reader = new StreamReader(context.Request.Body))
                            {
                                var path = await reader.ReadToEndAsync();
                                fileWatcherService.SetPath(path);
                            }

                            await context.Response.WriteAsync("Path set successfully.");
                        });
                    });
                }).UseUrls($"http://localhost:{availablePort}");
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<FileWatcherService>();
            })
            .Build();
        Console.CancelKeyPress += (sender, e) =>
       {
           Console.WriteLine("Shutting down the application...");
           e.Cancel = true;
       };

        host.Run();
    }
}
