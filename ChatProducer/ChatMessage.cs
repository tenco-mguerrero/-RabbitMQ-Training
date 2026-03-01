using Simone.Common.RabbitMQ.Models;

public class ChatMessage : IMessageHeader
{
    public string Type => nameof(ChatMessage);
    public required string CorrelationId { get; set; }
    public int Index { get; set; }
    public Queue? Queue { get; set; }
    public IEnumerable<(string CorrelationId, int Index)> RelatedCorrelationIds { get; set; } = Array.Empty<(string, int)>();
    
    public string User { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}