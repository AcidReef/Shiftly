using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shiftly.Models;
using Shiftly.Repositories;

namespace Shiftly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repo;

        public UserController(UserRepository repo) => _repo = repo;

        // Dostęp tylko dla managera
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [Authorize(Roles = "Manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            await _repo.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            await _repo.UpdateAsync(id, user);
            return NoContent();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        // Podgląd własnego profilu (dostępny dla każdego zalogowanego)
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userName = User.Identity?.Name;
            if (userName == null)
                return Unauthorized();

            var user = await _repo.GetByUserNameAsync(userName);
            return user is null ? NotFound() : Ok(user);
        }
    }
}