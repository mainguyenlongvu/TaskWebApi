using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskWebApi.Model;
using TaskWebApi.Repositories.Entities;
using TaskWebApi.Services;

namespace API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IAuthenticationService _authenticationService;

        public UserController(IUserService userService, IConfiguration config, IAuthenticationService authenticationService)
        {

            this._userService = userService;
            _config = config;
            _authenticationService = authenticationService;

        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("/api/[controller]/get-all-user")]
        public async Task<ActionResult<UserEntity>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/get-user-by-id")]
        public async Task<ActionResult<UserEntity>> GetUserById(int id)
        {
            try
            {
                var existingUser = await _userService.GetUserByIdAsync(id);

                if (existingUser == null)
                {
                    return NotFound("User does not exist");
                }
                return Ok(existingUser);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("/api/[controller]/login")]
        public async Task<ActionResult> Login([FromBody] RequestLoginModel requestLoginModel)
        {

            return Ok(_authenticationService.AuthenticatorAsync(requestLoginModel));
        }

        private string GenerateJWTToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin"), // Thêm các quyền của người dùng vào đây
                // Các thông tin khác của người dùng có thể được thêm vào đây
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Thời gian sống của token
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    

        [HttpPut]
        [Route("/api/[controller]/edit-user")]
        public async Task<ActionResult<UserEntity>> EditUser([FromRoute] int id , [FromBody] UserModel userModel)
        { 
            try
            {
                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null)
                    return NotFound("User does not exist");

                var updatedUser = await _userService.UpdateUserAsync(existingUser.Id, userModel);

                return Ok(updatedUser);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/api/[controller]/delete-user-by-id")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                var existingUser = await _userService.GetUserByIdAsync(id);

                if (existingUser == null)
                    return NotFound("User does not exist");

                 await _userService.DeleteUserAsync(id);

                 return Ok(existingUser);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
