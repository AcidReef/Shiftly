using MongoDB.Driver;
using Shiftly.Data;
using Shiftly.Models;
public class UserRepository
{
    private readonly MongoDbContext _ctx;

    public UserRepository(MongoDbContext ctx)
    {
        _ctx = ctx;
    }

    public Task<List<User>> GetAllAsync() => _ctx.Users.Find(_ => true).ToListAsync();
    public async Task<User?> GetByIdAsync(string id)
    { return await _ctx.Users.Find(u => u.Id == id).FirstOrDefaultAsync(); }

    public Task CreateAsync(User user) => _ctx.Users.InsertOneAsync(user);
    public Task UpdateAsync(string id, User user) => _ctx.Users.ReplaceOneAsync(u => u.Id == id, user);
    public Task DeleteAsync(string id) => _ctx.Users.DeleteOneAsync(u => u.Id == id);
    public Task<User?> GetByUserNameAsync(string userName) => _ctx.Users.Find(u => u.UserName == userName).FirstOrDefaultAsync();
}