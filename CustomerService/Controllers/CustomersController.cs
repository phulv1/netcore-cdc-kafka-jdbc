using CustomerService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{email}")]
    public async Task<ActionResult> UpdateCustomer(string email, [FromBody] UpdateCustomerCommand command)
    {
        await _mediator.Send(command.SetEmail(email));
        return Ok();
    }
}
