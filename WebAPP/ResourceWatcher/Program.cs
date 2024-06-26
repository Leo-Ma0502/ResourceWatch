using ResourceWatcher.Data;
using ResourceWatcher.Services;
using Microsoft.EntityFrameworkCore;
using ResourceWatcher.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<RabbitMQMessageService>();
builder.Services.AddHostedService<RabbitMQMessageService>();

builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddSingleton<IFileWatcherService, FileWatcherService>();
builder.Services.AddHttpClient();

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
