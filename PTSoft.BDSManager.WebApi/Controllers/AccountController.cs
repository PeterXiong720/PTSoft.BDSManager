using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PTSoft.BDSManager.WebApi.Dtos;
using PTSoft.BDSManager.WebApi.Services;

namespace PTSoft.BDSManager.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IJwtAuthManager _jwtAuthManager;
    
    public AccountController(IConfiguration configuration, IJwtAuthManager jwtAuthManager, ILogger<AccountController> logger)
    {
        _configuration = configuration;
        _jwtAuthManager = jwtAuthManager;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        // 验证用户账号密码是否正确
        string admin = _configuration["Admin:UserName"].ToLower();
        string password = _configuration["Admin:Password"];
        if (loginDto.UserName.ToLower() != admin || loginDto.Password.Trim() != password)
        {
            return Unauthorized();
        }

        // 生成JWT
        var role = "admin";
        var claims = new[]
        {
            new Claim(ClaimTypes.Name,loginDto.UserName),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtResult = _jwtAuthManager.GenerateTokens(loginDto.UserName, claims, DateTime.Now);
        _logger.LogInformation($"User [{loginDto.UserName}] logged in the system.");
        return Ok(new LoginResultDto
        {
            UserName = loginDto.UserName,
            Role = role,
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken.TokenString
        });
    }
    
    [Authorize]
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        var userName = User.Identity?.Name;
        _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("user")]
    public ActionResult GetCurrentUser()
    {
        return Ok(new LoginResultDto
        {
            UserName = User.Identity?.Name,
            Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty
        });
    }
    
    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenDto request)
    {
        try
        {
            var userName = User.Identity?.Name;
            _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return Unauthorized();
            }

            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
            _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
            return Ok(new LoginResultDto
            {
                UserName = userName,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
        }
    }
}