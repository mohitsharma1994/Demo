using Demo.BLL.IService;
using Demo.Infrastructure.Entity;
using Demo.Infrastructure.RequestModel;
using Demo.Infrastructure.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        public IConfiguration _configuration;
        public ApplicationController(UserManager<User> userManager, IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginResult = new LoginResult();

            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    loginResult.Message = "Invalid credentials";
                    loginResult.Status = HttpStatusCode.BadRequest;
                }
                else
                {
                    var tokenResponse = GenerateToken(user);
                    loginResult = tokenResponse;
                }

                await _userService.AddUserLoginLogAsync(loginResult.Status == HttpStatusCode.OK, loginResult.UserId);

            }
            catch (Exception ex)
            {
                loginResult.Message = ex.Message;
                loginResult.Status = HttpStatusCode.InternalServerError;
            }
            return Ok(loginResult);
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> Logout()
        {
            var result = new BaseResult();
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    result.Message = "User does not found.";
                    result.Status = HttpStatusCode.NotFound;
                    return Ok(result);
                }

                await _userService.AddUserLogoutLogAsync(user.Id);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = HttpStatusCode.InternalServerError;
                
            }
            return Ok(result);
        }


        private LoginResult GenerateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var expireTime = DateTime.Now.AddHours(Convert.ToDouble(_configuration["TokenExpiredHours"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expireTime,
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var tokenResponse = new LoginResult
            {
                Status = HttpStatusCode.OK,
                UserId = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                TokenExpireTime = expireTime
            };
            return tokenResponse;
        }
    }
}
