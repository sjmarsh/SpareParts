using Microsoft.AspNetCore.Mvc;
using SpareParts.API.Services;
using SpareParts.Shared.Models;

namespace SpareParts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticatinService;
        
        public UserController(IAuthenticationService authenticatinService)
        {
            _authenticatinService = authenticatinService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticationRequest authenticationRequest)
        {
            var response = _authenticatinService.Authenticate(authenticationRequest);
            if(response.IsAuthenticated)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(new { message = "Username or Password is incorrect." });
            }
        }
    }
}
