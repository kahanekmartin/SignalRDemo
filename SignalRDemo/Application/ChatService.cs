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
    
    public ChatService(IOpenAIService openAiService, IUserRepository userRepository)
    {
        this.openAiService = openAiService;
        this.userRepository = userRepository;
    }
    
    async Task<string> IChatService.Respond(string request, Guid userId)
    {
        await userRepository.AddMessage(userId,
            new Message { Content = request, Source = MessageSource.USER, Timestamp = DateTimeOffset.Now });

        var user = await userRepository.Get(userId);

        var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = ComposeChatMessages(user.Messages),
            Model = Models.ChatGpt3_5Turbo
        });

        ArgumentNullException.ThrowIfNull(completionResult.Choices);
        
        await userRepository.AddMessage(userId,
            new Message {
                Content = completionResult.Choices.First().Message.Content,
                Source = MessageSource.CHAT,
                Timestamp = DateTimeOffset.Now 
            });

        return completionResult.Choices.First().Message.Content;
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
    public Task<string> Respond(string request, Guid userId);
}