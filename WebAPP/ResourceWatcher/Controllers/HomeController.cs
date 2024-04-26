using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceWatcher.Models;
using ResourceWatcher.Services;

namespace ResourceWatcher.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RabbitMQMessageService _rabbitMQMessageService;

    public HomeController(ILogger<HomeController> logger, RabbitMQMessageService rabbitMQMessageService)
    {
        _logger = logger;
        _rabbitMQMessageService = rabbitMQMessageService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
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

}
