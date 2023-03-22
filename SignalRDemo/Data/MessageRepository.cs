using MongoDB.Driver;
using SignalRDemo.Model;

namespace SignalRDemo.Data;

public class MessageRepository : IMessageRepository
{
    private readonly IMongoCollection<Message> messages;
    
    public MessageRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("SignalRDemo");
        messages = database.GetCollection<Message>("Messages");
    }
    
    async Task IMessageRepository.Insert(Message message)
    {
        await messages.InsertOneAsync(message);
    }

    Task<List<Message>> IMessageRepository.Get() => messages.Find(_ => true).ToListAsync();
}

public interface IMessageRepository
{
    Task Insert(Message message);
    Task<List<Message>> Get();
}