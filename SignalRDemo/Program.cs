using MongoDB.Driver;
using NodeReact;
using OpenAI.GPT3.Extensions;
using SignalRDemo.Application;
using SignalRDemo.Data;
using SignalRDemo.Tests;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

//Configure services
services.AddMvc().AddRazorRuntimeCompilation();

var settings = MongoClientSettings.FromConnectionString(builder.Configuration["MongoDB:ConnectionString"]);

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddSingleton<IMongoClient>(config => new MongoClient(settings));
services.AddSingleton<IUserRepository, UserRepository>();

services.AddScoped<IChatService, ChatService>();
services.AddScoped<IUserAccessor, UserAccessor>();

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

        config.ConfigureSystemTextJsonPropsSerializer((_) => { });
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

app.UseRouting();
app.MapControllers();
        
app.UseOutputCache();

app.MapTestingApi();

app.Run();