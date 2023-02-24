using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleApi.Models;

namespace SimpleApi.Controllers;

[ApiController]
[Route("/")]
public class DefaultController : ControllerBase
{
    private readonly IOptionsMonitor<ReceiveConfig> _receiveConfig;
    private readonly IOptionsMonitor<SendConfig> _sendConfig;

    public DefaultController(IOptionsMonitor<ReceiveConfig> receiveConfig, IOptionsMonitor<SendConfig> sendConfig)
    {
        _receiveConfig = receiveConfig;
        _sendConfig = sendConfig;
    }

    [HttpGet]
    public ContentResult Get()
    {
        var endpoints = new StringBuilder();
        foreach (var request in _sendConfig.CurrentValue.Requests)
        {
            endpoints.Append($@"
                <tr>
                    <td>{request.Method}</td>
                    <td><a href=""{request.Uri}"">{request.Uri}</a></td>
                </tr>
            ");
        }

        var html = @$"
            <html>
                <head>
                    <title>Simple API</title>
                    <style>
                        th, td {{ padding-right: 8px; }}
                        h2 {{ margin-bottom: 0; }}
                        h3 {{ margin-bottom: 0; }}
                    </style>
                </head>
                <body style=""font-family: sans-serif;"">
                    <h1>Simple API</h1>
                    <h2>Simple API Endpoints</h2>
                    <table>
                        <tr>
                            <td><a href=""/receive"">{Request.Host}/receive</a></td>
                            <td>Receive a GET request and send the response text <code>{_receiveConfig.CurrentValue.Response}<code></td>
                        </tr>
                        <tr>
                            <td><a href=""/send"">{Request.Host}/send</a></td>
                            <td>Send requests to the following endpoints listed below:</td>
                        </tr>
                    </table>

                    <h3>Endpoints for /send</h3>
                    <table>
                        {endpoints.ToString()}
                    </table>
                </body>
            </html>
        ";
        return base.Content(html, "text/html");
    }
}