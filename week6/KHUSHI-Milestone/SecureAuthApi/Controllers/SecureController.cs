using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/secure-data")]
[Authorize]
public class SecureController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecureData()
    {
        return Ok(new
        {
            message = "Secure data accessed successfully.",
            data = new { user_id = User.Identity?.Name, secure_info = "This is some sensitive user data." }
        });
    }
}
