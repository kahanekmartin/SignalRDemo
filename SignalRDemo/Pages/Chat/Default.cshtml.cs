using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRDemo.Application;
using SignalRDemo.Model;

namespace SignalRDemo.Pages.Chat;

public class DefaultModel : PageModel
{
    private readonly IUserAccessor userAccessor;

    public DefaultModel(IUserAccessor userAccessor)
    {
        this.userAccessor = userAccessor;
    }

    public Lobby Model => new() { LogInUrl = "/api/users" };

    public IActionResult OnGet()
    {
        if (userAccessor.UserId.HasValue)
        {
            return Redirect("/chat");
        }

        return Page();
    }
}