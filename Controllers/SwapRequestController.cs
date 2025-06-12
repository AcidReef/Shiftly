using Microsoft.AspNetCore.Mvc;
using Shiftly.Models;
using Shiftly.Repositories;

namespace Shiftly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SwapRequestController : ControllerBase
    {
        private readonly SwapRequestRepository _repo;

        public SwapRequestController(SwapRequestRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var swap = await _repo.GetByIdAsync(id);
            return swap is null ? NotFound() : Ok(swap);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SwapRequest swapRequest)
        {
            await _repo.CreateAsync(swapRequest);
            return CreatedAtAction(nameof(GetById), new { id = swapRequest.Id }, swapRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, SwapRequest swapRequest)
        {
            await _repo.UpdateAsync(id, swapRequest);
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