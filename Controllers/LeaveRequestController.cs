using Microsoft.AspNetCore.Mvc;
using Shiftly.Models;
using Shiftly.Repositories;

namespace Shiftly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly LeaveRequestRepository _repo;

        public LeaveRequestController(LeaveRequestRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var lr = await _repo.GetByIdAsync(id);
            return lr is null ? NotFound() : Ok(lr);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequest leaveRequest)
        {
            await _repo.CreateAsync(leaveRequest);
            return CreatedAtAction(nameof(GetById), new { id = leaveRequest.Id }, leaveRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, LeaveRequest leaveRequest)
        {
            await _repo.UpdateAsync(id, leaveRequest);
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