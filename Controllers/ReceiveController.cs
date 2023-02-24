using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleApi.Models;

namespace SimpleApi.Controllers;

[ApiController]
[Route("/receive")]
public class ReceiveController : ControllerBase
{
    private readonly IOptionsMonitor<ReceiveConfig> _receiveConfig;

    public ReceiveController(IOptionsMonitor<ReceiveConfig> receiveConfig)
    {
        _receiveConfig = receiveConfig;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_receiveConfig.CurrentValue.Response);
    }
}