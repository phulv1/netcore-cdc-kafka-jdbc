using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using SharedService.Kafka.Consumer;

namespace NotificationService.Events.Handlers;

public class CustomerUpdatedEventHandler : IKafkaHandler<string, CustomerUpdatedEvent>
{
    private readonly NotificationDBContext _dbContext;

    public CustomerUpdatedEventHandler(NotificationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(string key, CustomerUpdatedEvent @event)
    {
        var user = await _dbContext.Customers
            .FirstOrDefaultAsync(s => string.Equals(s.Email, @event.Email, StringComparison.CurrentCultureIgnoreCase))
                ?? throw new ApplicationException("Email is not found.");
        user.FirstName = @event.FirstName;
        user.LastName = @event.LastName;
        user.PhoneNumber = @event.PhoneNumber;

        await _dbContext.SaveChangesAsync();
    }
}
