namespace SignalRDemo.Application;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId => (Guid?)(httpContextAccessor?.HttpContext?.Items["x-user"] ?? null);
}

public interface IUserAccessor
{
    public Guid? UserId { get; }
}