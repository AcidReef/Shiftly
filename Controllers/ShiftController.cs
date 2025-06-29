using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shiftly.Models;
using Shiftly.Repositories;
using Shiftly.Services;

namespace Shiftly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly ShiftService _shiftService;
        private readonly ShiftRepository _repo;

        public ShiftController(ShiftService shiftService, ShiftRepository repo)
        {
            _shiftService = shiftService;
            _repo = repo;
        }

        // Dostęp dla zalogowanych (wszyscy użytkownicy)
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.GetAllAsync());

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var shift = await _repo.GetByIdAsync(id);
            return shift is null ? NotFound() : Ok(shift);
        }

        // Dostęp tylko dla menedżerów
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(Shift shift)
        {
            if (!await _shiftService.IsUserAvailable(shift.UserId, shift.Start, shift.End))
                return BadRequest("User already has a shift at this time!");

            await _repo.CreateAsync(shift);
            return CreatedAtAction(nameof(GetById), new { id = shift.Id }, shift);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Shift shift)
        {
            await _repo.UpdateAsync(id, shift);
            return NoContent();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}