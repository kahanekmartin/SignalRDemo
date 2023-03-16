namespace SignalRDemo.Model;

public class Chat
{
    public string HubUrl { get; set; } = "";
    public User? LoggedUser { get; set; }
}