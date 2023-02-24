namespace SimpleApi.Models;

public class SendConfig
{
    public const string SectionName = "OrchestrationConfig";
    
    public IList<RequestDetails> Requests { get; set; } = new List<RequestDetails>();
}