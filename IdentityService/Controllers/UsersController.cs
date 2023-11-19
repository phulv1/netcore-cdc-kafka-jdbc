﻿using IdentityService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}
