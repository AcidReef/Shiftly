using Shiftly.Models;
using Shiftly.Repositories;

namespace Shiftly.Services;

public class LeaveRequestService
{
    private readonly LeaveRequestRepository _repo;
    private readonly NotificationService _notifier;

    public LeaveRequestService(
        LeaveRequestRepository repo,
        NotificationService notifier)
    {
        _repo = repo;
        _notifier = notifier;
    }

    public Task<List<LeaveRequest>> GetAllAsync()
        => _repo.GetAllAsync();

    public Task<LeaveRequest?> GetByIdAsync(string id)
        => _repo.GetByIdAsync(id);

    public async Task<LeaveRequest> CreateAsync(LeaveRequest lr)
    {
        lr.Status = "Pending";
        lr.SubmittedAt = DateTime.UtcNow;
        await _repo.CreateAsync(lr);
        return lr;
    }

    public async Task<LeaveRequest?> UpdateAsync(string id, LeaveRequest lr)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return null;
        await _repo.UpdateAsync(id, lr);
        return lr;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return false;
        await _repo.DeleteAsync(id);
        return true;
    }

    public async Task<LeaveRequest?> ApproveAsync(string id)
    {
        var lr = await _repo.GetByIdAsync(id);
        if (lr is null) return null;
        lr.Status = "Approved";
        await _repo.UpdateAsync(id, lr);
        await _notifier.SendLeaveRequestEventAsync(lr);
        return lr;
    }

    public async Task<LeaveRequest?> RejectAsync(string id)
    {
        var lr = await _repo.GetByIdAsync(id);
        if (lr is null) return null;
        lr.Status = "Rejected";
        await _repo.UpdateAsync(id, lr);
        await _notifier.SendLeaveRequestEventAsync(lr);
        return lr;
    }
}