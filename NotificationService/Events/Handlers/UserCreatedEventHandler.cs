using NotificationService.Data;
using NotificationService.Data.Entities;
using SharedService.Kafka.Consumer;

namespace NotificationService.Events.Handlers;

public class UserCreatedEventHandler : IKafkaHandler<string, UserCreatedEvent>
{
    private readonly NotificationDBContext _dbContext;

    public UserCreatedEventHandler(NotificationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(string key, UserCreatedEvent @event)
    {
        _dbContext.Customers.Add(new Customer
        {
            Id = @event.Id,
            Email = @event.Email,
            FirstName = @event.FirstName,
            LastName = @event.LastName,
        });

        await _dbContext.SaveChangesAsync();
    }
}
