namespace CustomerService.Data.Entities;

public class OutboxEvent
{
    public Guid Id { get; set; }
    public string AggregateType { get; set; }
    public Guid AggregateId { get; set; }
    public string Type { get; set; }
    public string Payload { get; set; }
}
