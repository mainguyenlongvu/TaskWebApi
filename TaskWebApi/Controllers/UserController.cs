using AutoMapper;
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

            return Ok(await _userService.LoginAsync(requestLoginModel));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/[controller]/register")]
        public async Task<ActionResult<UserEntity>> Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                var newUser = await _userService.Register(registerModel);

                return Ok(_mapper.Map<UserEntity>(newUser));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
