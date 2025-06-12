using Shiftly.Models;
using Shiftly.Data;
using MongoDB.Driver;

namespace Shiftly.Repositories
{
    public class ShiftRepository
    {
        private readonly MongoDbContext _context;

        public ShiftRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Shift>> GetAllAsync() =>
            await _context.Shifts.Find(_ => true).ToListAsync();

        public async Task<Shift?> GetByIdAsync(string id) =>
            await _context.Shifts.Find(s => s.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Shift shift) =>
            await _context.Shifts.InsertOneAsync(shift);

        // Dodaj inne metody: Update, Delete
        public Task UpdateAsync(string id, Shift shift) =>
            _context.Shifts.ReplaceOneAsync(s => s.Id == id, shift);

        public Task DeleteAsync(string id) =>
            _context.Shifts.DeleteOneAsync(s => s.Id == id);
    }
}