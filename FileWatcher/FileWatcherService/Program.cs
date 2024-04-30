using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

class Program
{
    public static void Main(string[] args)
    {
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
                }).UseUrls($"http://localhost:5000");
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
