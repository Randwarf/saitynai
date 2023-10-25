using API.Auth;
using API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Data.Dtos;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<GameUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<GameUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUser)
        {
            var user = await _userManager.FindByNameAsync(registerUser.UserName);
            if (user != null)
                return BadRequest("Request invalid - username already in use");

            var newUser = new GameUser
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
            };

            var userResult = await _userManager.CreateAsync(newUser, registerUser.Password);

            if (!userResult.Succeeded)
            {
                var stringBuilder = new StringBuilder();
                foreach (var error in userResult.Errors)
                {
                    stringBuilder.AppendLine(error.Description.ToString());
                }
                return BadRequest(stringBuilder.ToString());
            }

            await _userManager.AddToRoleAsync(newUser, GameRoles.User);

            return CreatedAtAction(nameof(Register), new UserDTO(newUser.Id, newUser.UserName, newUser.Email));
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginUserDTO loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.UserName);
            if (user == null)
                return BadRequest("User name or password is invalid");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUser.Password);
            if (!isPasswordValid)
                return BadRequest("User name or password is invalid");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);

            return Ok(new SuccessfulLoginDTO(accessToken));
        }
    }
}
