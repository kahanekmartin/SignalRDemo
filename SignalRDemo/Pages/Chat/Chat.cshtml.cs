using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRDemo.Application;
using SignalRDemo.Data;

namespace SignalRDemo.Pages.Chat;

public class ChatModel : PageModel
{
    private readonly IUserAccessor userAccessor;
    private readonly IUserRepository userRepository;

    public ChatModel(IUserAccessor userAccessor, IUserRepository userRepository)
    {
        this.userAccessor = userAccessor;
        this.userRepository = userRepository;
    }

    public Model.Chat Model { get; set; } = new Model.Chat();
    
    public async Task<IActionResult> OnGetAsync([FromQuery] bool stream = true)
    {
        if (!userAccessor.UserId.HasValue)
        { 
            return Redirect("/");
        }

        var user = await userRepository.Get(userAccessor.UserId!.Value);

        Model.HubUrl = "/chat-hub";
        Model.LoggedUser = user;
        Model.Stream = stream;

        return Page();
    }
}