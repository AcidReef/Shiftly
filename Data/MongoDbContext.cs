using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Shiftly.Models;

namespace Shiftly.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }

    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<Shift> Shifts => _db.GetCollection<Shift>("shifts");
        public IMongoCollection<SwapRequest> SwapRequests => _db.GetCollection<SwapRequest>("swaprequests");
        public IMongoCollection<User> Users => _db.GetCollection<User>("users");
        public IMongoCollection<LeaveRequest> LeaveRequests => _db.GetCollection<LeaveRequest>("leaverequests");
        // Dodasz więcej kolekcji, np. Users, LeaveRequests itd.
    }
}