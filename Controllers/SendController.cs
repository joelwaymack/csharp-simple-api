using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleApi.Models;

namespace SimpleApi.Controllers;

[ApiController]
[Route("/send")]
public class SendController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IOptionsMonitor<SendConfig> _sendConfig;

    public SendController(IHttpClientFactory clientFactory, IOptionsMonitor<SendConfig> sendConfig)
    {
        _clientFactory = clientFactory;
        _sendConfig = sendConfig;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var responseTasks = new List<Task<HttpResponseMessage>>();

        foreach (var request in _sendConfig.CurrentValue.Requests)
        {
            var client = _clientFactory.CreateClient();

            switch (request.Method.ToLower())
            {
                case "get":
                    responseTasks.Add(client.GetAsync(request.Uri));
                    break;
                case "post":
                    responseTasks.Add(client.PostAsync(request.Uri, null));
                    break;
                case "put":
                    responseTasks.Add(client.PutAsync(request.Uri, null));
                    break;
                case "delete":
                    responseTasks.Add(client.DeleteAsync(request.Uri));
                    break;
            }
        }

        var responses = await Task.WhenAll(responseTasks);
        var result = new OrchestrationResult();
        result.Results = new List<WebResponse>();

        foreach (var response in responses)
        {
            var webResponse = new WebResponse();
            webResponse.StatusCode = (int)response.StatusCode;
            webResponse.Uri = response?.RequestMessage?.RequestUri?.ToString() ?? string.Empty;
            webResponse.Method = response?.RequestMessage?.Method?.ToString() ?? string.Empty;
            webResponse.Headers = response?.Headers?.SelectMany(h => h.Value)?.ToList() ?? new List<string>();
            webResponse.Body = await (response?.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty));

            result.Results.Add(webResponse);
        }

        return Ok(result);
    }
}