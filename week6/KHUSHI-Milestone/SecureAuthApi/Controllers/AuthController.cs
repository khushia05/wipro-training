using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenService _tokenService;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null) return Unauthorized(new { error = "Invalid credentials" });

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) return Unauthorized(new { error = "Invalid credentials" });

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);

        return Ok(new
        {
            token,
            expires_in = 3600,
            user = new { id = user.Id, username = user.UserName, roles }
        });
    }

    [HttpPost("oauth")]
    public async Task<IActionResult> OAuth([FromBody] dynamic request)
    {
     
        var user = new User { UserName = "google_user" };
        var roles = new List<string> { "User" };
        var token = _tokenService.GenerateToken(user, roles);

        return Ok(new
        {
            token,
            expires_in = 3600,
            user = new { id = 2, username = user.UserName, roles }
        });
    }
}
