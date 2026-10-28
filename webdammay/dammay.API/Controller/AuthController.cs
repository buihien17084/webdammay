using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using MediatR;
using webdammay.dammay.Application.Command.Auth;
namespace webdammay.dammay.API.Controller;

[ApiController]
[Route("api/auth")]

    public class AuthController: ControllerBase
    {
    private readonly IMediator _mediator;
    public AuthController (IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var userId=await _mediator.Send(command);
        return Ok(new {Message="Tạo tài khoản thành công", UserId=userId});
    }
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = _mediator.Send(command);
            return BadRequest(new { Message = "Đăng nhập thành công", Data = result });
        }
        catch(UnauthorizedAccessException ex)
        {
            return Unauthorized(new {Message=ex.Message});
        }
    }
}

