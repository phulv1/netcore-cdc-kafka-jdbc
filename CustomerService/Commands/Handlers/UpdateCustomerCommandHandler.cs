using CustomerService.Data;
using CustomerService.Data.Entities;
using CustomerService.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CustomerService.Commands.Handlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly CustomerDBContext _dbContext;

    public UpdateCustomerCommandHandler(CustomerDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    async Task IRequestHandler<UpdateCustomerCommand>.Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(s => string.Equals(s.Email, command.Email, StringComparison.CurrentCultureIgnoreCase), cancellationToken) 
                ?? throw new ApplicationException("Email is not found.");
        customer.FirstName = command.FirstName;
        customer.LastName = command.LastName;
        customer.Address = command.Address;
        customer.PhoneNumber = command.PhoneNumber;
        customer.Gender = command.Gender;
        customer.BirthDate = command.BirthDate;

        var outboxEvent = new OutboxEvent
        {
            Id = Guid.NewGuid(),
            AggregateId = customer.Id,
            AggregateType = "Customer",
            Type = "CustomerUpdated",
            Payload = JsonConvert.SerializeObject(new CustomerUpdatedEvent
            {
                Email = customer.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Address = customer.Address,
                BirthDate = customer.BirthDate,
                PhoneNumber = customer.PhoneNumber,
                Gender = customer.Gender
            })
        };

        _dbContext.Customers.Update(customer);

        _dbContext.OutboxEvents.Add(outboxEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
