using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using SignalRDemo.Data;
using SignalRDemo.Model;

namespace SignalRDemo.Application;

public class ChatService : IChatService
{
    private readonly IOpenAIService openAiService;
    private readonly IUserRepository userRepository;
    private readonly IMessageRepository messageRepository;
    
    public ChatService(IOpenAIService openAiService, 
                       IUserRepository userRepository, 
                       IMessageRepository messageRepository)
    {
        this.openAiService = openAiService;
        this.userRepository = userRepository;
        this.messageRepository = messageRepository;
    }

    async Task<Message> IChatService.Respond(string request, Guid userId)
    {
        Message message = new() { Id = Guid.NewGuid(),Content = request, Source = MessageSource.USER, Timestamp = DateTimeOffset.Now };
        await messageRepository.Insert(message);
        await userRepository.AddMessage(userId, message);

        var user = await userRepository.Get(userId);

        var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = ComposeChatMessages(user.Messages),
            Model = Models.ChatGpt3_5Turbo
        });

        ArgumentNullException.ThrowIfNull(completionResult.Choices);

        Message response = new ()
            {
                Id = Guid.NewGuid(),
                Content = completionResult.Choices.First().Message.Content,
                Source = MessageSource.CHAT,
                Timestamp = DateTimeOffset.Now
            };

        await userRepository.AddMessage(userId, response);
        await messageRepository.Insert(response);

        return response;
    }
    
    async IAsyncEnumerable<Message> IChatService.RespondAsStream(string request, Guid userId)
    {
        Message message = new() { Id = Guid.NewGuid(),Content = request, Source = MessageSource.USER, Timestamp = DateTimeOffset.Now };
        await messageRepository.Insert(message);
        await userRepository.AddMessage(userId, message);

        var user = await userRepository.Get(userId);

        var completionResult = openAiService.ChatCompletion.CreateCompletionAsStream(new ChatCompletionCreateRequest
        {
            Messages = ComposeChatMessages(user.Messages),
            Model = Models.ChatGpt3_5Turbo,
            Stream = true
        });

        string lastCompletion = "";
        
        await foreach (var completion in completionResult)
        {
            if (completion.Successful)
            {
                lastCompletion += completion.Choices.First().Message.Content;
                
                Message streamResponse = new ()
                {
                    Id = Guid.NewGuid(),
                    Content = lastCompletion,
                    Source = MessageSource.CHAT,
                    Timestamp = DateTimeOffset.Now
                };

                yield return streamResponse;
            }
        }
        
        Message response = new ()
        {
            Id = Guid.NewGuid(),
            Content = lastCompletion,
            Source = MessageSource.CHAT,
            Timestamp = DateTimeOffset.Now
        };
        
        await userRepository.AddMessage(userId, response);
        await messageRepository.Insert(response);
    }

    async Task<List<Message>> IChatService.GetHistory(Guid? userId)
    {
        if (userId.HasValue)
        {
            var user = await userRepository.Get(userId.Value);

            return user.Messages;
        }

        return await messageRepository.Get();
    }

    private List<ChatMessage> ComposeChatMessages(List<Message> userMessages) =>
        userMessages.OrderBy(x => x.Timestamp)
                    .Select(x => x.Source == MessageSource.CHAT 
                        ? ChatMessage.FromAssistant(x.Content) 
                        : ChatMessage.FromUser(x.Content))
                    .ToList();
}

public interface IChatService
{
    Task<Message> Respond(string request, Guid userId);
    IAsyncEnumerable<Message> RespondAsStream(string request, Guid userId);
    Task<List<Message>> GetHistory(Guid? userId = null);
}