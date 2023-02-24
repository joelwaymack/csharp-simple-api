namespace SimpleApi.Models;

public class WebResponse
{
    public int StatusCode { get; set; } = 200;
    public string Uri { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";
    public IList<string> Headers { get; set; } = new List<string>();
    public string Body { get; set; } = string.Empty;
}