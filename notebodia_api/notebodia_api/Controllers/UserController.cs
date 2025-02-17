using Microsoft.AspNetCore.Mvc;
using notebodia_api.Dto;
using notebodia_api.Models;
using notebodia_api.Response;
using notebodia_api.Services;
using notebodia_api.Types;

namespace notebodia_api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        //Testing Purposes
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        //Testing Purposes
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetMe()
        {
            var user = await _userService.GetMe();
            return ApiOk(user);
        }
        [HttpPost("signup")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UserSignup(CreateUserPayload payload)
        {
            var user = await _userService.UserSignup(payload);
            return ApiOk(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UserLogin(CreateUserPayload payload)
        {
            var user = await _userService.UserLogin(payload);
            return ApiOk(user);
        }

        [HttpPost("signout")]
        public async Task<ActionResult<ApiResponse<object>>> UserSignout()
        {
            var res = await _userService.UserSignout();
            return ApiSimpleSuccess();
        }

    }
}