using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using trsaints_frontend_api.Entities;

namespace trsaints_frontend_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController: ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] User model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
            return Ok(model);

        return BadRequest(ModelState);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserToken>> Login([FromBody] User userInfo)
    {
        var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return Ok(BuildToken(userInfo));
        
        ModelState.AddModelError(string.Empty, "invalid login");
        return BadRequest(ModelState);
    }

    private UserToken BuildToken(User userInfo)
    {
        var jwtIssuer = _configuration["Jwt:Issuer"]
            .Replace("{JwtIssuer}", _configuration.GetValue<string>("JWT_ISSUER"));
        var jwtAudience = _configuration["Jwt:Audience"]
            .Replace("{JwtAudience}", _configuration.GetValue<string>("JWT_AUDIENCE"));
        var jwtAuthKey = _configuration["Jwt:Key"]
            .Replace("{AuthKey}", _configuration.GetValue<string>("JWT_AUTH_KEY"));
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
            new Claim("trsaints", "https://www.trsantos.tech"),
            new Claim(JwtRegisteredClaimNames.Iss, jwtIssuer),
            new Claim(JwtRegisteredClaimNames.Aud, jwtAudience),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(2);
        
        var token = new JwtSecurityToken(claims: claims, expires:expiration, signingCredentials: credentials);

        return new UserToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
