using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceWatcher.Models;
using ResourceWatcher.Services;
using ResourceWatcher.DTOs;

namespace ResourceWatcher.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RabbitMQMessageService _rabbitMQMessageService;
    private readonly IFileWatcherService _fileWatcherService;
    private readonly IMessageService _messageService;


    public HomeController(ILogger<HomeController> logger, RabbitMQMessageService rabbitMQMessageService, IFileWatcherService fileWatcherService, IMessageService messageService)
    {
        _logger = logger;
        _rabbitMQMessageService = rabbitMQMessageService;
        _fileWatcherService = fileWatcherService;
        _messageService = messageService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> History()
    {
        var historyImgs = new ImageViewModel
        {
            Images = await _messageService.GetAllMessagesAsync()
        };
        return View(historyImgs);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult GetLatestMessages()
    {
        var messages = _rabbitMQMessageService.ReceiveMessage();
        return Json(messages);
    }

    [HttpPost]
    public async Task<IActionResult> SetWatchPath([FromBody] PathDTO pathDto)
    {
        if (pathDto != null && !string.IsNullOrEmpty(pathDto.Path))
        {
            var result = await _fileWatcherService.SetWatchPathAsync(pathDto.Path);
            if (result)
            {
                return Json(new { message = "Path set successfully." });
            }
            return BadRequest("Error with fileWatcherService.");
        }
        return BadRequest("Path parameter is null or empty.");
    }
}
