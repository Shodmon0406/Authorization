using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Infrastructure.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("get-all-user")]
    [Authorize]
    public async Task<IActionResult> GetAllUser()
    {
        var response = await _service.GetAllUserAsync();
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequestDto model)
    {
        var response = await _service.LoginAsync(model);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequestDto model)
    {
        var response = await _service.RegisterAsync(model);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("unblocked")]
    public async Task<IActionResult> UnBlockedAsync(int id)
    {
        var response = await _service.UnBlockedUserAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("blocked")]
    public async Task<IActionResult> BlockedAsync(int id)
    {
        var response = await _service.BlockedUserAsync(id);
        return StatusCode(response.StatusCode, response);
    }
}