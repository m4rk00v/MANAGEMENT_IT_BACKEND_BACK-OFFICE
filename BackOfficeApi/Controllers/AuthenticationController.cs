

using BussinessLogic.Repositories.Interfaces;
using BussinessLogic.Services.Interfaces;
using Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace BackOfficeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var token = await _authenticationService.LoginService(loginRequest.Email,loginRequest.Password);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return Unauthorized();
        }
    }
}