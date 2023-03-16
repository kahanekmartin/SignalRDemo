using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Application;
using SignalRDemo.Data;

namespace SignalRDemo.Tests;

public static class Tests
{
    public static void MapTestingApi(this WebApplication app)
    {
        var tests = app.MapGroup("/api/tests");
        
        tests.MapPost("/openai", async (CompletionRequest request, IChatService chatService) =>
        {
            var result = await chatService.Respond(request.Request, request.UserId);

            return Results.Ok(result);
        });

        tests.MapPost("/users/{id}", async (Guid id, [FromBody] UserRequest userRequest, IUserRepository userRepository) =>
        {
            var user = await userRepository.Create(id, userRequest.Name);
            
            ArgumentNullException.ThrowIfNull(user);

            return Results.Created($"/users/{id}", user);
        });
        
        tests.MapGet("/users", async (IUserRepository userRepository) =>
        {
            var users = await userRepository.Get();

            return Results.Ok(users);
        });
        
        tests.MapGet("/users/{id}", async (Guid id, IUserRepository userRepository) =>
        {
            var user = await userRepository.Get(id);

            return Results.Ok(user);
        });
    }
}

public class CompletionRequest
{
    public string Request { get; set; } = "";
    public Guid UserId { get; set; } = new Guid();
}

public class UserRequest
{
    public string Name { get; set; } = "";
}