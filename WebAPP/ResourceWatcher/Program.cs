using ResourceWatcher.Data;
using ResourceWatcher.Services;
using Microsoft.EntityFrameworkCore;
using ResourceWatcher.Hubs;

Console.WriteLine("Please enter the port number for the file watcher service:");
string portInput = Console.ReadLine();
var port = ValidatePort(portInput) ? portInput : "5000";

static bool ValidatePort(string input)
{
    return int.TryParse(input, out int port) && port > 1024 && port < 65535;
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<RabbitMQMessageService>();
builder.Services.AddHostedService<RabbitMQMessageService>();

builder.Services.AddSingleton<IFileWatcherService, FileWatcherService>();
builder.Services.AddHttpClient<IFileWatcherService, FileWatcherService>(client =>
    {
        client.BaseAddress = new Uri($"http://localhost:{port}/");
    });

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<MessageHub>("/messageHub");

Console.CancelKeyPress += (sender, e) =>
{
    Console.WriteLine("Exiting...");
    e.Cancel = true;
};

app.Run();
