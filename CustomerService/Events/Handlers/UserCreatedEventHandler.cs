using CustomerService.Data;
using CustomerService.Data.Entities;
using SharedService.Kafka.Consumer;

namespace CustomerService.Events.Handlers;

public class UserCreatedEventHandler : IKafkaHandler<string, UserCreatedEvent>
{
    private readonly CustomerDBContext _dbContext;

    public UserCreatedEventHandler(CustomerDBContext dbContext)
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
