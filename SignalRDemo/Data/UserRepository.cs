using MongoDB.Driver;
using SignalRDemo.Model;

namespace SignalRDemo.Data;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> users;
    
    public UserRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("SignalRDemo");
        users = database.GetCollection<User>("Users");
    }
    
    public async Task<User?> Create(Guid id, string name)
    {
        await users.InsertOneAsync(new User { Id = id, Name = name });

        return await Get(id);
    }

    public Task<User> Get(Guid id)
        => users.Find(us => us.Id == id).FirstOrDefaultAsync();  

    public Task<List<User>> Get()
        => users.Find(_ => true).ToListAsync();  

    public async Task AddMessage(Guid userId, Message message)
    {
        var user = await Get(userId);
        
        user.Messages.Add(message);

        await users.ReplaceOneAsync(us => us.Id == userId, user);
    }
}

public interface IUserRepository
{
    Task<User?> Create(Guid id, string name);
    Task<User> Get(Guid id);
    Task<List<User>> Get();
    Task AddMessage(Guid userId, Message message);
}