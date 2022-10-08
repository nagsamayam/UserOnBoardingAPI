using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserOnBoarding.Data;
using UserOnBoarding.Services.UserServices;

namespace UserOnBoarding.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(DataContext context, IUserService userService, ILogger<UserController> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("users/{page}")]
        public async Task<ActionResult<List<User>>> Get(int page = 1)
        {
            var response = await _userService.GetAllUsers(page);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(await _userService.GetAllUsers(page));
        }

    }
}
