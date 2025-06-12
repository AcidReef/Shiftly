using MongoDB.Driver;
using Shiftly.Data;
using Shiftly.Models;

namespace Shiftly.Repositories
{
    public class LeaveRequestRepository
    {
        private readonly MongoDbContext _context;

        public LeaveRequestRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveRequest>> GetAllAsync() =>
            await _context.LeaveRequests.Find(_ => true).ToListAsync();

        public async Task<LeaveRequest?> GetByIdAsync(string id) =>
            await _context.LeaveRequests.Find(lr => lr.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(LeaveRequest leaveRequest) =>
            await _context.LeaveRequests.InsertOneAsync(leaveRequest);

        public Task UpdateAsync(string id, LeaveRequest leaveRequest) =>
            _context.LeaveRequests.ReplaceOneAsync(lr => lr.Id == id, leaveRequest);

        public Task DeleteAsync(string id) =>
            _context.LeaveRequests.DeleteOneAsync(lr => lr.Id == id);
    }
}