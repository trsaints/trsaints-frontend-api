using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using trsaints_frontend_api.Authorization.Constants;
using trsaints_frontend_api.Entities;

namespace trsaints_frontend_api.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpPost("signup")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] User model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(ModelState);

        var roleResult = await _userManager.AddToRoleAsync(user, ResourceOperationsConstants.RoleUsers);
        
        if (!roleResult.Succeeded)
            return BadRequest(ModelState);

        return Ok(model);
    }

    [HttpPost("signin")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserToken>> Login([FromBody] User userInfo)
    {
        var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return Ok(await BuildToken(userInfo));
        
        ModelState.AddModelError(string.Empty, "invalid signin");
        return BadRequest(ModelState);
    }

private async Task<UserToken> BuildToken(User userInfo)
{
    var jwtIssuer = _configuration["Jwt:Issuer"]
        .Replace("{JwtIssuer}", _configuration.GetValue<string>(JwtAuthenticationConstants.JwtIssuer));
    var jwtAudience = _configuration["Jwt:Audience"]
        .Replace("{JwtAudience}", _configuration.GetValue<string>(JwtAuthenticationConstants.JwtAudience));
    var jwtAuthKey = _configuration["Jwt:Key"]
        .Replace("{JwtIssuerSigningKey}", _configuration.GetValue<string>(JwtAuthenticationConstants.JwtIssuerSigningKey));
    
    var user = await _userManager.FindByEmailAsync(userInfo.Email);
    var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
        new("trsaints", "https://www.trsantos.tech"),
        new(JwtRegisteredClaimNames.Iss, jwtIssuer),
        new(JwtRegisteredClaimNames.Aud, jwtAudience),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var roles = await _userManager.GetRolesAsync(user);
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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
