using MongoDB.Driver;
using Shiftly.Data;
using Shiftly.Models;

namespace Shiftly.Repositories
{
    public class SwapRequestRepository
    {
        private readonly MongoDbContext _context;

        public SwapRequestRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<SwapRequest>> GetAllAsync() =>
            await _context.SwapRequests.Find(_ => true).ToListAsync();

        public async Task<SwapRequest?> GetByIdAsync(string id) =>
            await _context.SwapRequests.Find(sr => sr.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(SwapRequest swapRequest) =>
            await _context.SwapRequests.InsertOneAsync(swapRequest);

        public Task UpdateAsync(string id, SwapRequest swapRequest) =>
            _context.SwapRequests.ReplaceOneAsync(sr => sr.Id == id, swapRequest);

        public Task DeleteAsync(string id) =>
            _context.SwapRequests.DeleteOneAsync(sr => sr.Id == id);
    }
}