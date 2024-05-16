using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskWebApi.CoreHelper.Helper;
using TaskWebApi.Enum;
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
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IConfiguration config, IAuthenticationService authenticationService, IMapper mapper)
        {

            this._userService = userService;
            _config = config;
            _authenticationService = authenticationService;
            _mapper = mapper;

        }


        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("/api/[controller]/get-all-user")]
        //[Authorize(Roles = AppRole.Admin)]
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
        public async Task<ActionResult<UserEntity>> GetUserById(string id)
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
            var result = await _userService.LoginAsync(requestLoginModel);

            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Wrong!!!!");
            }

            return Ok(result);
        }


        [HttpPost]
        [Route("/api/[controller]/register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                var result = await _userService.Register(registerModel);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/[controller]/confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string otp)
        {
            return await _userService.ConfirmEmail(email, otp);
        }


        [HttpPut]
        [Route("/api/[controller]/edit-user")]
        public async Task<ActionResult<UserEntity>> EditUser([FromRoute] string id , [FromBody] UserModel userModel)
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
        public async Task<ActionResult> DeleteUser([FromRoute] string id)
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
