using LogiTrack.Infrastructure.Identity;

namespace LogiTrack.Infrastructure.Authentication;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(LogiTrackUser user);
}