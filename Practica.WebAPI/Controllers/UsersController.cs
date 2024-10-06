using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica.Application.Services;
using Practica.Domain.Entities;

namespace Practica.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {

            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors if model is invalid
            }

            try
            {
                await _userService.AddUserAsync(user);
                return NoContent(); // Assuming you have a GetUser method
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // 409 Conflict if email already exists
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed, e.g., using ILogger
                return StatusCode(500, "Internal server error"); // 500 for other exceptions
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() { 
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> RemoveUserByID(int id) {

            try
            {
                await _userService.RemoveUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateUserByID(int id, [FromBody] User user) {

            if (user == null) return BadRequest(new { Message = "User data is required." });

            if (id != user.Id) return BadRequest(new { Message = "User ID in the URL and body must match." });

            try
            {
                await _userService.UpdateUserByIdAsync(id, user);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

    }
}
