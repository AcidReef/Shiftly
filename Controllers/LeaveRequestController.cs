using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shiftly.Models;
using Shiftly.Repositories;
using Shiftly.Services;
using Shiftly.DTOs;

namespace Shiftly.Controllerss
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly LeaveRequestService _svc;

        public LeaveRequestController(LeaveRequestService svc) => _svc = svc;

        [Authorize] // Użytkownik musi być zalogowany, żeby zobaczyć swoje zgłoszenia
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetAllAsync());

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var lr = await _svc.GetByIdAsync(id);
            return lr is null ? NotFound() : Ok(lr);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequestCreateDto dto)
        {
            if (dto.Start > dto.End)
                return BadRequest("Data zakończenia nie może być przed datą rozpoczęcia.");

            // Ew: Czy UserId zgadza się z zalogowanym użytkownikiem
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (userId != dto.UserId) return Forbid();

            var leaveRequest = new LeaveRequest
            {
                UserId = dto.UserId,
                Start = dto.Start,
                End = dto.End,
                Reason = dto.Reason,
                Status = "Pending",
                SubmittedAt = DateTime.UtcNow
            };

            var created = await _svc.CreateAsync(leaveRequest);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize] // Edycja tylko po zalogowaniu
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, LeaveRequest leaveRequest)
        {
            var updated = await _svc.UpdateAsync(id, leaveRequest);
            return updated is null ? NotFound() : NoContent();
        }

        [Authorize] // Usuwanie tylko po zalogowaniu
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _svc.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // === TE ENDPOINTY TYLKO DLA MANAGERÓW ===
        [Authorize(Roles = "Manager")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(string id)
        {
            var lr = await _svc.ApproveAsync(id);
            return lr is null ? NotFound() : Ok(lr);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(string id)
        {
            var lr = await _svc.RejectAsync(id);
            return lr is null ? NotFound() : Ok(lr);
        }
    }
}