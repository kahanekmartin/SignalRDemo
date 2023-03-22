using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Model;

namespace SignalRDemo.Application;

public class ChatHub : Hub<IChatHub>
{
    private readonly IChatService chatService;

    public ChatHub(IChatService chatService)
    {
        this.chatService = chatService;
    }

    public async Task Register(Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        
        var chatHistory = await chatService.GetHistory(userId);
        
        await Clients.Groups(userId.ToString()).Registered(chatHistory);
    }
    
    public async Task Send(Guid userId, string message)
    {
        var response = await chatService.Respond(message, userId);

        await Clients.Groups(userId.ToString()).Response(response);
    }
}

public interface IChatHub
{
    Task Registered(List<Message> messages);
    Task Response(Message message);
}