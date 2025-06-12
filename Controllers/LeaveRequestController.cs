using Microsoft.AspNetCore.Mvc;
using Shiftly.Models;
using Shiftly.Repositories;
using Shiftly.Services;

namespace Shiftly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly LeaveRequestService _svc;

        public LeaveRequestController(LeaveRequestService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var lr = await _svc.GetByIdAsync(id);
            return lr is null ? NotFound() : Ok(lr);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequest leaveRequest)
        {
            var created = await _svc.CreateAsync(leaveRequest);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, LeaveRequest leaveRequest)
        {
            var updated = await _svc.UpdateAsync(id, leaveRequest);
            return updated is null ? NotFound() : NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _svc.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(string id)
        {
            var lr = await _svc.ApproveAsync(id);
            return lr is null ? NotFound() : Ok(lr);
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(string id)
        {
            var lr = await _svc.RejectAsync(id);
            return lr is null ? NotFound() : Ok(lr);
        }
    }
}
// namespace Shiftly.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class LeaveRequestController : ControllerBase
//     {
//         private readonly LeaveRequestRepository _repo;

//         public LeaveRequestController(LeaveRequestRepository repo) => _repo = repo;

//         [HttpGet]
//         public async Task<IActionResult> GetAll() =>
//             Ok(await _repo.GetAllAsync());

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetById(string id)
//         {
//             var lr = await _repo.GetByIdAsync(id);
//             return lr is null ? NotFound() : Ok(lr);
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create(LeaveRequest leaveRequest)
//         {
//             leaveRequest.Status = "Pending";
//             leaveRequest.SubmittedAt = DateTime.UtcNow;

//             await _repo.CreateAsync(leaveRequest);
//             return CreatedAtAction(nameof(GetById), new { id = leaveRequest.Id }, leaveRequest);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(string id, LeaveRequest leaveRequest)
//         {
//             await _repo.UpdateAsync(id, leaveRequest);
//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> Delete(string id)
//         {
//             await _repo.DeleteAsync(id);
//             return NoContent();
//         }

//         [HttpPost("{id}/approve")]
//         public async Task<IActionResult> Approve(string id)
//         {
//             var leave = await _repo.GetByIdAsync(id);
//             if (leave is null) return NotFound();

//             leave.Status = "Approved";
//             await _repo.UpdateAsync(id, leave);
//             return Ok(leave);
//         }

//         [HttpPost("{id}/reject")]
//         public async Task<IActionResult> Reject(string id)
//         {
//             var leave = await _repo.GetByIdAsync(id);
//             if (leave is null) return NotFound();

//             leave.Status = "Rejected";
//             await _repo.UpdateAsync(id, leave);
//             return Ok(leave);
//         }
//     }
// }