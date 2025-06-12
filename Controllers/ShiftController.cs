using Microsoft.AspNetCore.Mvc;
using Shiftly.Models;
using Shiftly.Repositories;

namespace Shiftly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly ShiftRepository _repo;

        public ShiftController(ShiftRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var shift = await _repo.GetByIdAsync(id);
            return shift is null ? NotFound() : Ok(shift);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Shift shift)
        {
            await _repo.CreateAsync(shift);
            return CreatedAtAction(nameof(GetById), new { id = shift.Id }, shift);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Shift shift)
        {
            await _repo.UpdateAsync(id, shift);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}