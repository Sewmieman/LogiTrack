using Microsoft.AspNetCore.Identity;

namespace LogiTrack.Infrastructure.Identity;

public class LogiTrackUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}