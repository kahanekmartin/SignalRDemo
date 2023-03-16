using NodeReact;
using OpenAI.GPT3.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

//Configure services
services.AddMvc().AddRazorRuntimeCompilation();

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

app.Run();