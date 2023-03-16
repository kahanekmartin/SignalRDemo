using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace SignalRDemo.Application;

public class ChatService : IChatService
{
    private readonly IOpenAIService openAiService;
    
    public ChatService(IOpenAIService openAiService)
    {
        this.openAiService = openAiService;
    }
    
    async Task<string> IChatService.Respond(string request, Guid userId)
    {
        var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromUser(request)
            },
            Model = Models.ChatGpt3_5Turbo
        });

        ArgumentNullException.ThrowIfNull(completionResult.Choices);
        
        return completionResult.Choices.First().Message.Content;
    }
}

public interface IChatService
{
    public Task<string> Respond(string request, Guid userId);
}