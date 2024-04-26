using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

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
                });
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<FileWatcherService>();
            })
            .Build();

        host.Run();
    }
}
