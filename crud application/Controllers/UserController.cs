using crud_application.Models;
using crud_application.service;
using Microsoft.AspNetCore.Mvc;

namespace crud_application.Controllers
{
    public class UserController:ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserModel user)
        {
           
            var CurrentUser=await _userService.SignIn(user);
            return Ok(CurrentUser);
        }
    }
}
