using Microsoft.AspNetCore.Mvc;
using Shiftly.Models;
using Shiftly.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Shiftly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SwapRequestController : ControllerBase
    {
        private readonly SwapRequestRepository _repo;

        public SwapRequestController(SwapRequestRepository repo) => _repo = repo;

        // Tylko manager może widzieć wszystkie swapy (możesz podzielić, ale uprośćmy)
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var swap = await _repo.GetByIdAsync(id);
            return swap is null ? NotFound() : Ok(swap);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(SwapRequest swapRequest)
        {
            // Użytkownik może wysłać swap tylko ze swoim UserId — najlepiej waliduj na backendzie!
            await _repo.CreateAsync(swapRequest);
            return CreatedAtAction(nameof(GetById), new { id = swapRequest.Id }, swapRequest);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, SwapRequest swapRequest)
        {
            await _repo.UpdateAsync(id, swapRequest);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{id}/accept")]
        public async Task<IActionResult> Accept(string id)
        {
            var swap = await _repo.GetByIdAsync(id);
            if (swap is null) return NotFound();

            swap.Status = "Accepted";
            await _repo.UpdateAsync(id, swap);
            return Ok(swap);
        }

        [Authorize]
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(string id)
        {
            var swap = await _repo.GetByIdAsync(id);
            if (swap is null) return NotFound();

            swap.Status = "Rejected";
            await _repo.UpdateAsync(id, swap);
            return Ok(swap);
        }

        // NOWY ENDPOINT: Cofnięcie przez managera
        [Authorize(Roles = "Manager")]
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            var swap = await _repo.GetByIdAsync(id);
            if (swap is null) return NotFound();

            swap.Status = "CancelledByManager";
            await _repo.UpdateAsync(id, swap);
            return Ok(swap);
        }
    }
    
}