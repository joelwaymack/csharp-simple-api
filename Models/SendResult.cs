namespace SimpleApi.Models;

public class OrchestrationResult
{
    public IList<WebResponse> Results { get; set; } = new List<WebResponse>();
}