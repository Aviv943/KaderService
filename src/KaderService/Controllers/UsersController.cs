using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KaderService.Services.Models;
using KaderService.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KaderService.Services.Models.AuthModels;
using KaderService.Contracts;
using KaderService.Contracts.Requests;
using KaderService.Services.Constants;
using Microsoft.AspNetCore.Http;

namespace KaderService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : MyControllerBase
    {
        private readonly UsersService _service;

        public UsersController(UsersService service, UserManager<User> userManager)
            : base(userManager)
        {
            _service = service;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            TokenInfo tokenInfo = await _service.LoginAsync(model);
            return Ok(tokenInfo);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            User registeredUser = await _service.RegisterAsync(model);

            if (registeredUser != null)
            {
                return Ok(new Response
                {
                    Status = "Success",
                    Message = "User created successfully!"
                });
            }

            return BadRequest(new Response
            {
                Status = "Failed",
                Message = "User creation failed! Please check user details and try again."
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _service.GetUsersAsync());
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAdminsAsync()
        {
            return Ok(await _service.GetAdminsAsync());
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            return Ok(await _service.GetUserViewAsync(userId));
        }

        [HttpPut("role/{userId}")]
        [Authorize(Policy = "GroupManager")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRoleAsync(string userId, string newRole)
        {
            await _service.PutRoleAsync(userId, newRole);
            return Ok();
        }

        [HttpPost("role/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostRoleAsync(string roleName)
        {
            await _service.PostRoleAsync(roleName);
            return Ok();
        }

        [HttpPost("{userId}/rating")]
        [Authorize]
        public async Task<IActionResult> AddRatingAsync(string userId, AddNewRatingRequest request)
        {
            await _service.AddRatingAsync(userId, request.NewRating);
            return Ok();
        }

        [HttpDelete("role/{roleName}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteRoleAsync(string roleName)
        {
            await _service.DeleteRoleAsync(roleName);
            return Ok();
        }

        [HttpPost("image")]
        [Authorize]
        public async Task<IActionResult> UpdateUserImageAsync([FromQuery] string userId)
        {
            User user = await GetRelevantUserAsync(userId);
            IFormFile file = Request.Form.Files.First();
            string userImage = await _service.UpdateUserImageAsync(user, file);

            return Ok(userImage);
        }
    }
}