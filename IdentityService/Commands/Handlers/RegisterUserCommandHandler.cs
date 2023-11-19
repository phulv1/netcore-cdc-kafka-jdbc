using IdentityService.Data.Entities;
using IdentityService.Data;
using IdentityService.Events;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MediatR;

namespace IdentityService.Commands.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IdentityDBContext _dbContext;
    public RegisterUserCommandHandler(IdentityDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    async Task IRequestHandler<RegisterUserCommand>.Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AsNoTracking()
                .AnyAsync(s => string.Equals(s.Email, command.Email, StringComparison.CurrentCultureIgnoreCase), cancellationToken))
            throw new ApplicationException("Email is already exist.");

        var user = new User
        {
            Id = command.Id,
            Password = command.Password,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
        };

        var outboxEvent = new OutboxEvent
        {
            Id = Guid.NewGuid(),
            AggregateId = command.Id,
            AggregateType = "User",
            Type = "UserCreated",
            Payload = JsonConvert.SerializeObject(new UserCreatedEvent
            {
                Id = user.Id,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
            })
        };

        _dbContext.Users.Add(user);

        _dbContext.OutboxEvents.Add(outboxEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
