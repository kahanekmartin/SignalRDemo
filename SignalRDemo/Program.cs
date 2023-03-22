using MongoDB.Driver;
using NodeReact;
using OpenAI.GPT3.Extensions;
using SignalRDemo.Application;
using SignalRDemo.Data;
using SignalRDemo.Tests;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

//Configure services
var settings = MongoClientSettings.FromConnectionString(builder.Configuration["MongoDB:ConnectionString"]);

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddSingleton<IMongoClient>(config => new MongoClient(settings));
services.AddSingleton<IUserRepository, UserRepository>();
services.AddSingleton<IMessageRepository, MessageRepository>();
services.AddSignalR();

services.AddScoped<IChatService, ChatService>();
services.AddScoped<IUserAccessor, UserAccessor>();

services.AddRazorPages().AddRazorRuntimeCompilation();

services.AddNodeReact(
    config =>
    {
        config.EnginesCount = 2;
        config.ConfigureOutOfProcessNodeJSService(o =>
        {
            o.NumRetries = 0;
            o.InvocationTimeoutMS = 10000;
        });
        config.AddScriptWithoutTransform("~/dist/server.js");
        config.UseDebugReact = true;
        config.UseServerSideRendering = true;
        config.ConfigureSystemTextJsonPropsSerializer(_ => { });
    });

services.AddOutputCache(options =>
{
    options.AddBasePolicy(build => build.Cache());
});

services.AddOpenAIService();

// Configure
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCookiePolicy();

app.Use(async (context, next) =>
{
    string? userCookie = context.Request.Cookies["x-user"];

    if (!string.IsNullOrEmpty(userCookie))
    {
        context.Items["x-user"] = Guid.Parse(userCookie);
    }

    await next();
});

app.UseRouting();
app.MapControllers();
app.MapRazorPages();
app.MapHub<ChatHub>("/chat-hub");
        
app.UseOutputCache();

app.MapPost("/api/users", async (UserRequest request, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor) =>
{
    string? userCookie = httpContextAccessor.HttpContext!.Request.Cookies["x-user"];

    if (!string.IsNullOrEmpty(userCookie))
    {
        return Results.Redirect("/chat");
    }

    var userId = CreateNewUserId(httpContextAccessor.HttpContext!);

    httpContextAccessor.HttpContext.Items["x-user"] = userId;

    var user = await userRepository.Create(userId, request.Name);

    return Results.Redirect("/chat");

    static Guid CreateNewUserId(HttpContext context)
    {
        var newBasketId = Guid.NewGuid();

        var options = new CookieOptions
        {
            Secure = true,
            SameSite = SameSiteMode.Lax,
            IsEssential = true
        };

        context.Response.Cookies.Append("x-user", newBasketId.ToString(), options);

        return newBasketId;
    }
});


app.MapTestingApi();

app.Run();