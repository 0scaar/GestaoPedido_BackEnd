using MF.OrderManagement.Application.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MF.OrderManagement.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IConfiguration config, ITokenService tokens) : ControllerBase
{
    public sealed record LoginRequest(string Username, string Password);
    public sealed record LoginResponse(string Token, int ExpiresInSeconds);

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest req)
    {
        var user = config["Auth:Username"];
        var pass = config["Auth:Password"];

        if (!string.Equals(req.Username, user, StringComparison.Ordinal) ||
            !string.Equals(req.Password, pass, StringComparison.Ordinal))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = tokens.GenerateToken(req.Username, roles: new[] { "User" });
        var expiresMin = int.TryParse(config["Jwt:ExpirationMinutes"], out var m) ? m : 60;

        return Ok(new LoginResponse(token, expiresMin * 60));
    }
}