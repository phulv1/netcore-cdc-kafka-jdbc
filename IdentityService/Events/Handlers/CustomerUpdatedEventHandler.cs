using IdentityService.Data;
using Microsoft.EntityFrameworkCore;
using SharedService.Kafka.Consumer;

namespace IdentityService.Events.Handlers;

public class CustomerUpdatedEventHandler : IKafkaHandler<string, CustomerUpdatedEvent>
{
    private readonly IdentityDBContext _dbContext;

    public CustomerUpdatedEventHandler(IdentityDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(string key, CustomerUpdatedEvent @event)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(s => string.Equals(s.Email, @event.Email, StringComparison.CurrentCultureIgnoreCase))
                ?? throw new ApplicationException("Email is not found.");
        user.FirstName = @event.FirstName;
        user.LastName = @event.LastName;

        await _dbContext.SaveChangesAsync();
    }
}
