namespace VOD.API.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using VOD.API.Filters;
    using VOD.Domain.Requests.User;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;

    [Authorize]
    [Route("api/v1/users")]
    [ApiController]
    [JsonException]
    public class UserController : ControllerBase
    {
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private readonly IUserService _userService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            if (claim == null) return Unauthorized();

            UserResponse token = await _userService.GetUserAsync(new GetUserRequest { Email = claim.Value });
            
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            TokenResponse token = await _userService.SignInAsync(request);

            if (token == null) return BadRequest();

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            UserResponse user = await _userService.SignUpAsync(request);
            
            if (user == null) return BadRequest();
            
            return CreatedAtAction(nameof(Get), new { }, null);
        }
    }
}